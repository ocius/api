using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;

namespace ociusApi
{
    public class Query
    {
        public static QueryRequest CreateLatestDronesRequest(string resource)
        {
            var date = DateTime.UtcNow.Date.ToShortDateString();

            return new QueryRequest
            {
                TableName = resource,
                KeyConditionExpression = "#date = :date",
                ExpressionAttributeNames = new Dictionary<string, string> { { "#date", "Date" } },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> { { ":date", new AttributeValue { S = date } } },
                ScanIndexForward = false,
                Limit = 2
            };
        }

        public static QueryRequest CreateDroneByDayRequest(string resource)
        {
            var date = DateTime.UtcNow.Date.ToShortDateString();

            return new QueryRequest
            {
                TableName = resource,
                KeyConditionExpression = "#date = :date",
                ExpressionAttributeNames = new Dictionary<string, string> { { "#date", "Date" } },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> { { ":date", new AttributeValue { S = date } } }
            };
        }

        public static QueryRequest CreateDroneByTimeRequest(string timespan, string resource)
        {
            var date = DateTime.UtcNow.Date.ToShortDateString();

            return new QueryRequest
            {
                TableName = resource,
                KeyConditionExpression = "#date = :date and #timespan > :timespan ",
                ExpressionAttributeNames = new Dictionary<string, string> { { "#timespan", "Timestamp" }, { "#date", "Date" } },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    { ":timespan", new AttributeValue { N = timespan } },
                    { ":date", new AttributeValue { S = date } },
                }
            };
        }
    }
}
