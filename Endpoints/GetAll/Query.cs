using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;

namespace ociusApi
{
    public class Query
    {
        public static QueryRequest CreateSingleDroneRequest(string resource, string name)
        {
            Console.WriteLine("============ query");

            var queryRequest = CreateDroneByNameRequest(resource, name);
            queryRequest.ScanIndexForward = false;
            queryRequest.Limit = 1;
            return queryRequest;
        }


        public static QueryRequest CreateDroneByNameRequest(string resource, string name)
        {
            var date = DateTime.UtcNow.Date.ToShortDateString();
            var table = resource.Contains("location") ? "DroneLocations" : "DroneSensors";

            Console.WriteLine("============ table", table);

            return new QueryRequest
            {
                TableName = table,
                IndexName = "Date-Name-index",
                KeyConditionExpression = "#date = :date and #name = :name",
                ExpressionAttributeNames = new Dictionary<string, string> { { "#date", "Date" }, { "#name", "Name" } },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":date", new AttributeValue { S = date } },
                    { ":name", new AttributeValue { S = name } }
                }
            };
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
