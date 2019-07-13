using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ociusApi
{
    public class DroneResponse
    {
        public List<Drone> Drones { get; set; }
    }

    public class Drone
    {
        public string Name { get; set; }
        public string Timestamp { get; set; }
        public string Data { get; set; }
    }

    public static class Database
    {
        private static readonly AmazonDynamoDBClient client = new AmazonDynamoDBClient();

        public async static Task<string> GetByTimespan(string timespan)
        {
            var query = GetQuery(timespan);
            var queryRequest = CreateQueryRequest(query);
            var queryResponse = await client.QueryAsync(queryRequest);
            return CreateDroneResponse(queryResponse);
        }

        private static string GetQuery(string timeSpan)
        {
            if (timeSpan == "day")
            {
                return DateTime.UtcNow.Date.ToShortDateString();
            }

            return "live";
        }

        private static QueryRequest CreateQueryRequest(string query)
        {
            return new QueryRequest
            {

                TableName = "TimeSeriesDroneData",
                KeyConditionExpression = "#dt = :query",
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    {"#dt", "Date"}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":query", new AttributeValue { S = query } }
                }
            };
        }

        private static string CreateDroneResponse(QueryResponse queryResponse)
        {
            var droneResponse = new DroneResponse();
            var drones = new List<Drone>();
            droneResponse.Drones = drones;

            if (!IsValidResponse(queryResponse)) return "The query response was null or empty";

            foreach (var item in queryResponse.Items)
            {
                var drone = CreateDrone(item);
                droneResponse.Drones.Add(drone);
            }

            return JsonConvert.SerializeObject(droneResponse);
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
                if (key == "Data") drone.Data = value?.S ?? "";
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
