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
            var rawData = await Database.ReadAsync(rawDataTableName);
            var cleanData = Drone.TransformData(rawData);
            await Database.InsertAsync(cleanData);
        }

        public class File {
            public List<string> Vehicles {get;}
        }

        public static class Drone
        {
            public static string TransformData(string rawData)
            {
                //get to json object
                //parse itturn 

                var json = JsonConvert.DeserializeObject(rawData) as JObject;
                var response = (json["Response"]);
                var file = response["File"];
                var foo = JsonConvert.DeserializeObject(file.ToString()) as File;
                //file can contain many vehicles
                var vehicle = file["Vehicle"];
                //pull out the name, the timestamp, and the vehicle json?
                return json.ToString();
            }
        }

        public static class Database
        {
            public static async Task<string> ReadAsync(string tableName)
            {
                var table = Table.LoadTable(client, tableName);
                var search = table.Query("Bruce", new QueryFilter("timestamp", QueryOperator.GreaterThan, 0));
                var results = await search.GetRemainingAsync();
                return results.First().ToJson();
            }

            public async static Task InsertAsync(string json)
            {
                var table = Table.LoadTable(client, "ClientData");
                var item = Document.FromJson(json);
                item["name"] = "Bruce";
                item["timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                await table.PutItemAsync(item);
            }
        }
    }
}



