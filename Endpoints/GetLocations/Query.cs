using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;

namespace GetLocations
{
    public class Query
    {
        public static QueryRequest CreateSingleDroneRequest()
        {
            var timespan = DateTime.UtcNow.Date.ToShortDateString();
            var queryRequest = CreateDroneByTimespanRequest(timespan);
            queryRequest.ScanIndexForward = false;
            queryRequest.Limit = 2;
            return queryRequest;
        }

        public static QueryRequest CreateDroneByTimespanRequest(string timespan)
        {
            return new QueryRequest
            {
                TableName = "DroneLocations",
                KeyConditionExpression = "#dt = :timespan",
                ExpressionAttributeNames = new Dictionary<string, string> { { "#dt", "Date" } },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> { { ":timespan", new AttributeValue { S = timespan } } }
            };
        }
    }
}
