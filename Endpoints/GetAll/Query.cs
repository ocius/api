using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;

namespace ociusApi
{
    public class Query
    {
        public static QueryRequest CreateLatestDronesRequest(string resource)
        {
            var Date = DateTime.UtcNow.ToString("yyyyMMdd");

            return new QueryRequest
            {
                TableName = resource,
                KeyConditionExpression = "#date = :date",
                ExpressionAttributeNames = new Dictionary<string, string> { { "#date", "Date" } },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> { { ":date", new AttributeValue { N = Date } } },
                ScanIndexForward = false,
                Limit = 2
            };
        }

        public static QueryRequest CreateDroneByTimeRequest(string date, long timestamp, string resource)
        {
            return new QueryRequest
            {
                TableName = resource,
                KeyConditionExpression = "#date = :date and #timespan > :timespan ",
                ExpressionAttributeNames = new Dictionary<string, string> { { "#timespan", "Timestamp" }, { "#date", "Date" } },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    { ":timespan", new AttributeValue { N = timestamp.ToString() } },
                    { ":date", new AttributeValue { N = date } },
                }
            };
        }
    }
}
