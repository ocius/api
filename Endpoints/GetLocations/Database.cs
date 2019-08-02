using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetLocations
{
    public static class Database
    {
        private static readonly AmazonDynamoDBClient client = new AmazonDynamoDBClient();

        public async static Task<string> GetLatest()
        {
            var singleDroneRequest = CreateSingleDroneRequest();
            var queryResponse = await client.QueryAsync(singleDroneRequest);
            return CreateDroneResponse(queryResponse);
        }

        public async static Task<string> GetByTimespan(string timespan)
        {
            var time = GetQuery(timespan);
            var dronesByTimespanRequest = CreateDroneByTimespanRequest(time);
            var queryResponse = await client.QueryAsync(dronesByTimespanRequest);
            return CreateDroneResponse(queryResponse);
        }

        private static string GetQuery(string timeSpan)
        {
            if (timeSpan == "day")
            {
                return DateTime.UtcNow.Date.ToShortDateString();
            }

            return "hour";
        }

        private static QueryRequest CreateSingleDroneRequest()
        {
            return new QueryRequest
            {

                TableName = "DroneLocations",
                KeyConditionExpression = "#dt = :timespan",
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    {"#dt", "Date"}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":timespan", new AttributeValue { S = timespan } }
                }
            };
        }

        private static QueryRequest CreateDroneByTimespanRequest(string timespan)
        {
            return new QueryRequest
            {

                TableName = "DroneLocations",
                KeyConditionExpression = "#dt = :timespan",
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    {"#dt", "Date"}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":timespan", new AttributeValue { S = timespan } }
                }
            };
        }

        private static string CreateDroneResponse(QueryResponse queryResponse)
        {
            var drones = new List<Drone>();

            if (!IsValidResponse(queryResponse)) return "There were no results for that time range";

            foreach (var item in queryResponse.Items)
            {
                var drone = CreateDrone(item);
                drones.Add(drone);
            }

            return JsonConvert.SerializeObject(drones);
        }

        private static bool IsValidResponse(QueryResponse queryResponse)
        {
            return queryResponse != null &&
                    queryResponse.Items != null &&
                    queryResponse.Items.Any();
        }
        

        private static Drone CreateDrone(Dictionary<string, AttributeValue> attributes)
        {
            var drone = new Drone();

            foreach (KeyValuePair<string, AttributeValue> kvp in attributes)
            {
                var key = kvp.Key;
                var value = kvp.Value;

                if (key == "Timestamp") drone.Timestamp = value?.N ?? "";
                if (key == "Lat") drone.Lat = value?.S ?? "";
                if (key == "Lon") drone.Lon = value?.S ?? "";
                if (key == "Name") drone.Name = value?.S ?? "";
            }

            return drone;
        }


        //live
        //minute
        //hour
        //day
        //week
        //year
        //all
    }
}
