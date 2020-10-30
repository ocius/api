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
        private static int Date => int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));

        public async static Task InsertAsync(string json, string tableName, long timestamp)
        {
            var table = Table.LoadTable(client, tableName);
            var item = Document.FromJson(json);

            item["Date"] = Date;
            item["Timestamp"] = timestamp;

            await table.PutItemAsync(item);
        }

        public static async Task<Dictionary<string, string>> GetCameras()
        {
            var cameraQuery = CreateCameraQuery();
            var response = await client.QueryAsync(cameraQuery);
            return GetValuesFromResponse(response);
        }

        private static Dictionary<string, string> GetValuesFromResponse(QueryResponse queryResponse)
        {
            if (!IsValidResponse(queryResponse))
            {
                Console.WriteLine("Invalid camera query response, returning empty dictionary");
                return new Dictionary<string, string>();
            }

            var value = new Dictionary<string, string>();

            foreach (var item in queryResponse.Items)
            {
                var (name, cameraUrls) = ParseCameraResponse(item);
                Console.WriteLine($"Found camera for {name} with with URL {cameraUrls}");
                value.TryAdd(name, cameraUrls);
            }

            return value;
        }

        private static (string, string) ParseCameraResponse(Dictionary<string, AttributeValue> attributes)
        {
            var name = string.Empty;
            var cameras = string.Empty;

            foreach (KeyValuePair<string, AttributeValue> attribute in attributes)
            {
                if (attribute.Key == "Name") name = attribute.Value?.S ?? "";

                if (attribute.Key == "Cameras") cameras = attribute.Value?.S ?? "";
            }

            return (name, cameras);
        }

        private static bool IsValidResponse(QueryResponse queryResponse)
        {
            return queryResponse != null && queryResponse.Items != null && queryResponse.Items.Any();
        }


        private static QueryRequest CreateCameraQuery()
        {
            var date = DateTime.UtcNow.Date.ToShortDateString();

            return new QueryRequest
            {
                TableName = "CameraImageUrls",
                KeyConditionExpression = "#date = :date",
                ExpressionAttributeNames = new Dictionary<string, string> { { "#date", "Date" } },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> { { ":date", new AttributeValue { S = date } } },
                ScanIndexForward = false,
                Limit = 10 //TODO: make this handle any number of drone cameras
            };
        }
    }
}