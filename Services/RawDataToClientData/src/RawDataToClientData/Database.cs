using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static async Task<Dictionary<string, List<string>>> GetCameras()
        {
            var cameraQuery = CreateCameraQuery();
            var response = await client.QueryAsync(cameraQuery);
            return GetValuesFromResponse(response);
        }

        public static Dictionary<string, List<string>> GetValuesFromResponse(QueryResponse queryResponse)
        {
            if (!IsValidResponse(queryResponse)) return new Dictionary<string, List<string>>();

            var value = new Dictionary<string, List<string>>();

            foreach (var item in queryResponse.Items)
            {
                var (foo,bar) = GetCameras(item);
                value.Add(foo, bar);
            }

            return value;
        }

        public static (string, List<string>) GetCameras(Dictionary<string, AttributeValue> attributes)
        {
            string name = string.Empty;
            List<string> cameras = new List<string>();

            foreach (KeyValuePair<string, AttributeValue> attribute in attributes)
            {
                if (attribute.Key == "Name")
                {
                    name = attribute.Value?.S ?? "";
                }

                if (attribute.Key == "Cameras")
                {
                    var cameraValue = attribute.Value?.S ?? "";
                    cameras = cameraValue.Split(",").ToList();
                }
            }

            return (name, cameras);
        }

        private static bool IsValidResponse(QueryResponse queryResponse)
        {
            return queryResponse != null && queryResponse.Items != null && queryResponse.Items.Any();
        }

        private static string Date => DateTime.UtcNow.Date.ToShortDateString();

        private static QueryRequest CreateCameraQuery()
        {

            return new QueryRequest
            {
                TableName = "CameraImageUrls",
                KeyConditionExpression = "#date = :date",
                ExpressionAttributeNames = new Dictionary<string, string> { { "#date", "Date" } },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> { { ":date", new AttributeValue { S = Date } } },
                ScanIndexForward = false,
                Limit = 2
            };
        }
    }
}
