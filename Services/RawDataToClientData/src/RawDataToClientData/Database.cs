using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Threading.Tasks;

namespace RawDataToClientData
{
    public static class Database
    {
        private static readonly AmazonDynamoDBClient client = new AmazonDynamoDBClient();

        public async static Task InsertAsync(string json, string tableName, long timestamp)
        {
            var table = Table.LoadTable(client, tableName);
            var item = Document.FromJson(json);

            item["Date"] = DateTime.UtcNow.Date.ToShortDateString();
            item["Timestamp"] = timestamp;

            await table.PutItemAsync(item);
        }
    }
}
