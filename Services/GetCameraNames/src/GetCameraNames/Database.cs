using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GetCameraNames
{
    public static class Database
    {
        private static readonly AmazonDynamoDBClient client = new AmazonDynamoDBClient();
        private static int Date => int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));

        public async static Task InsertAsync(string json, string tableName, long timestamp)
        {
            var table = Table.LoadTable(client, tableName);
            var item = Document.FromJson(json);

            item["Date"] = Date;
            item["Timestamp"] = timestamp;

            await table.PutItemAsync(item);
        }
    }
}
