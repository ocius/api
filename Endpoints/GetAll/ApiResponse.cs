using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ociusApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ociusApi
{
    public class ApiResponse
    {
        #region Properties

        [JsonProperty("isBase64Encoded")]
        public bool IsBase64Encoded = false;

        [JsonProperty("statusCode")]
        public int StatusCode { get; private set; }

        [JsonProperty("body")]
        public string Body { get; private set; }

        [JsonProperty("headers")]
        public IDictionary<string, string> Headers { get; private set; }

        #endregion

        private static string Today => GetDate(0);
        private static string Yesterday => GetDate(-1);
        private static List<string> SupportedDroneNames => Database.GetSupportedDrones();

        public static async Task<ApiResponse> GetLatest(string resource)
        {
            var drones = new List<DroneSensors>();
            foreach (var droneName in SupportedDroneNames)
            {
                var databaseResponse = await Database.GetLatest(resource, droneName);
                drones.add(drone);
            }
            var droneJson = JsonConvert.SerializeObject(drones);
            return CreateResponse(droneJson);
        }

        public static async Task<ApiResponse> GetByTimespan(JToken queryString, string resource)
        {
            var timespan = queryString.ToObject<Timespan>();

            if (!Database.IsValidTimePeriod(timespan.Value)) return null;

            var ticks = Database.GetTimespan(timespan.Value);

            Console.WriteLine("TICKS " + ticks);

            var utcMidnight = DateTime.Today.Ticks;

            Console.WriteLine("MIDNIGHT " + utcMidnight);

            var databaseResponse = await Database.GetByTimespan(Today, timespan.Value, resource);

            if (ticks < utcMidnight)
            {
                Console.WriteLine("FROM YESTERDAY");
                var dataFromYesterday = await Database.GetByTimespan(Yesterday, timespan.Value, resource);
                databaseResponse.Items.AddRange(dataFromYesterday.Items);
            }
            var drone = DroneFactory.GetDroneType(resource);
            var droneJson = drone.ToJson(databaseResponse);
            return CreateResponse(droneJson);
        }

        private static ApiResponse CreateApiResponse(string json)
        {
            var headers = new Dictionary<string, string>() { { "Access-Control-Allow-Origin", "*" } };
            return new ApiResponse { StatusCode = 200, Body = json, Headers = headers };
        }


        public static string GetDate(int offset)
        {
            return DateTime.UtcNow.AddDays(offset).ToString("yyyyMMdd");
        }
    }
}
