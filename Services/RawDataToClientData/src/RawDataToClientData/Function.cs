using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace RawDataToClientData
{
    public class Function
    {
        private static readonly AmazonDynamoDBClient client = new AmazonDynamoDBClient();

        public async Task FunctionHandler(ILambdaContext context)
        {
            var rawDataTableName = "RawData";
            var rawData = await Database.ReadAsync(client, rawDataTableName);
            var cleanData = Drone.TransformData(rawData);
            await Database.InsertAsync(client, cleanData);
        }
    }

    public class Drone
    {
        public string Lat {get;}
        public string Lon {get;}  //Long is a reserved word     
        public Drone(string lat, string lon) 
        {
            Lat = lat;
            Lon = lon;
        }

        public static string TransformData(string rawData)
        {
            var json = JsonConvert.DeserializeObject(rawData) as JObject;
            var response = json["Response"];
            var file = response["File"];
            var vehicles = file["Vehicle"];


            var lat = vehicles.First()["mavpos"]["lat"].ToString();
            var lon = vehicles.First()["mavpos"]["lon"].ToString();
            var drone = new Drone(lat, lon);
            return JsonConvert.SerializeObject(drone);
        }
    }

    public static class Database
    {
        public static async Task<string> ReadAsync(AmazonDynamoDBClient client, string tableName)
        {
            var table = Table.LoadTable(client, tableName);
            var search = table.Query("Bruce", new QueryFilter("timestamp", QueryOperator.GreaterThan, 0));
            var results = await search.GetRemainingAsync();
            return results.First().ToJson();
        }

        public async static Task InsertAsync(AmazonDynamoDBClient client, string json)
        {
            var table = Table.LoadTable(client, "ClientData");
            var item = Document.FromJson(json);
            item["name"] = "Bruce";
            item["timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            await table.PutItemAsync(item);
        }
    }
}



