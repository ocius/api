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

        private static bool parseDroneSensitivityQuery(QueryResponse queryResponse)
        {
            if (queryResponse == null || queryResponse.Items == null || !queryResponse.Items.Any())
            {
                Console.WriteLine($"Could not query data for drone sensitivty");
            }
            Dictionary<string, AttributeValue> item = queryResponse.Items[0];
            AttributeValue av = null;
            if (item.TryGetValue("Sensitivity", out av))
            {
                return av.BOOL;
            }
            return false;
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

        public static async Task<bool> GetDroneSensitivity(string droneName)
        {
            var droneSensitivityQuery = CreateDroneSensitivityQuery(droneName);
            var response = await client.QueryAsync(droneSensitivityQuery);
            return parseDroneSensitivityQuery(response);
        }
    }
}
