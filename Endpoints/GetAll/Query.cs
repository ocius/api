using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;

namespace ociusApi
{
    public class Query
    {
        public static QueryRequest CreateSingleDroneRequest()
        {
            var queryRequest = CreateDroneByDayRequest();
            queryRequest.ScanIndexForward = false;
            queryRequest.Limit = 2;
            return queryRequest;
        }

        public static QueryRequest CreateDroneByDayRequest()
        {
            var date = DateTime.UtcNow.Date.ToShortDateString();

            return new QueryRequest
            {
                TableName = "DroneSensors",
                KeyConditionExpression = "#date = :date",
                ExpressionAttributeNames = new Dictionary<string, string> { { "#date", "Date" } },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> { { ":date", new AttributeValue { S = date } } }
            };
        }

        public static QueryRequest CreateDroneByTimeRequest(string timespan)
        {
            var date = DateTime.UtcNow.Date.ToShortDateString();

            return new QueryRequest
            {
                TableName = "DroneSensors", //change this
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
