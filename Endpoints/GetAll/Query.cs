using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;

namespace ociusApi
{
    public class Query
    {
        private static string Today => GetDate(0);
        private static string Yesterday => GetDate(-1);

        public static QueryRequest CreateLatestDronesRequest(string resource)
        {
            return new QueryRequest
            {
                TableName = resource,
                KeyConditionExpression = "#date = :date",
                ExpressionAttributeNames = new Dictionary<string, string> { { "#date", "Date" } },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> { { ":date", new AttributeValue { N = Today } } },
                ScanIndexForward = false,
                Limit = 2
            };
        }

        public static QueryRequest CreateDroneByTimeRequest(string timespan, string resource)
        {
            return new QueryRequest
            {
                TableName = resource,
                KeyConditionExpression = "#date > :date and #timespan > :timespan ",
                ExpressionAttributeNames = new Dictionary<string, string> { { "#timespan", "Timestamp" }, { "#date", "Date" } },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    { ":timespan", new AttributeValue { N = timespan } },
                    { ":date", new AttributeValue { N = Yesterday } },
                }
            };
        }

        private static string GetDate(int offset)
        {
            var date = DateTime.UtcNow.AddDays(offset).Date.ToShortDateString();
            return date.Replace("/", string.Empty);
        }
    }
}
