using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RawDataToClientData.Repositories
{
    public static class SensitivityRepository
    {
        private static readonly AmazonDynamoDBClient client = new AmazonDynamoDBClient();

        public static async Task<bool> GetDroneSensitivity(string droneName)
        {
            var droneSensitivityQuery = CreateDroneSensitivityQuery(droneName);
            var response = await client.QueryAsync(droneSensitivityQuery);
            return IsDroneSensitive(response);
        }

        private static QueryRequest CreateDroneSensitivityQuery(string droneName)
        {
            return new QueryRequest
            {
                TableName = "DroneStatus",
                KeyConditionExpression = "#DroneName = :DroneName",
                ExpressionAttributeNames = new Dictionary<string, string> { { "#DroneName", "DroneName" } },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> { { ":DroneName", new AttributeValue { S = droneName } } },
                ScanIndexForward = false,
            };
        }

        private static bool IsDroneSensitive(QueryResponse queryResponse)
        {
            if (queryResponse == null || queryResponse.Items == null || !queryResponse.Items.Any())
            {
                Console.WriteLine($"Could not query data for drone sensitivty");
            }
            Dictionary<string, AttributeValue> item = queryResponse.Items[0];

            return item.ContainsKey("isSensitive") ? item["isSensitive"].BOOL : false;
        }
    }
}
