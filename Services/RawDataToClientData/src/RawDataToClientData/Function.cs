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
            var cleanData = TransformData(rawData);
            await Database.InsertAsync(client, cleanData);
        }

        public static string TransformData(string rawData)
        {
            var vehicles = GetVehiclesArray(rawData);
            var drones = vehicles.Select(v => CreateDrone(v)).ToList();
            var owner = new Owner(drones);
            return JsonConvert.SerializeObject(owner);
        }

        private static Drone CreateDrone(JToken vehicle){
            var lat = vehicle["mavpos"]["lat"]?.ToString();
            var lon = vehicle["mavpos"]["lon"]?.ToString();
            return new Drone(lat, lon);
        }

        private static JToken GetVehiclesArray(string rawData){
            var json = JsonConvert.DeserializeObject(rawData) as JObject;
            var data = json["Response"]["File"];
            return data["Vehicle"];
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
    }

    public class Owner
    {
        public List<Drone> Drones {get;}

        public Owner(List<Drone> drones)
        {
            Drones = drones;
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



