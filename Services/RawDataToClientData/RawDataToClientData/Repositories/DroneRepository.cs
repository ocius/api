using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RawDataToClientData
{
    public static class DroneRepository
    {
        private static readonly AmazonDynamoDBClient client = new AmazonDynamoDBClient();
        private static int Date => int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));

        public async static Task InsertAsyncDeprecated(string json, string tableName, long timestamp)
        {
            var table = Table.LoadTable(client, tableName);
            var item = Document.FromJson(json);

            item["Date"] = Date;
            item["Timestamp"] = timestamp;

            await table.PutItemAsync(item);
        }

        public async static Task InsertDrone(string json, string tableName, long timestamp)
        {
            var table = Table.LoadTable(client, tableName);
            var item = Document.FromJson(json);
            item["DroneName+Date"] = item["Name"]+Date;
            item["Date"] = Date;
            item["Timestamp"] = timestamp;

            await table.PutItemAsync(item);
        }

        
    }
}