using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;

namespace GetDrones
{
    public class Query
    {
        public static QueryRequest CreateSingleDroneRequest(string resource)
        {
            Console.WriteLine("============ query");

            var queryRequest = CreateDroneByDayRequest(resource);
            queryRequest.ScanIndexForward = false;
            queryRequest.Limit = 2;
            return queryRequest;
        }

        public static QueryRequest CreateDroneByDayRequest(string resource)
        {
            var date = DateTime.UtcNow.Date.ToShortDateString();
            var table = resource.Contains("location") ? "DroneLocations" : "DroneSensors";

            Console.WriteLine("============ table", table);


            return new QueryRequest
            {
                TableName = table,
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
                TableName = resource, //change this
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
