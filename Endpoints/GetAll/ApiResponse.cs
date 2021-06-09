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

        public static async Task<ApiResponse> GetLatest()
        {
            Console.WriteLine("Loading latest data");
            var supportedDroneNames = await Database.GetSupportedDrones();
            var drones = await Database.GetLatest(Today, supportedDroneNames);
            var dronesJson = JsonConvert.SerializeObject(drones);
            return CreateApiResponse(dronesJson);
        }

        public static async Task<ApiResponse> GetByTimespan(JToken queryString)
        {
            Console.WriteLine("Loading timespan data");
            var supportedDroneNames = await Database.GetSupportedDrones();
            var timespan = queryString.ToObject<Timespan>();

            // TODO: remove null return -> empty list
            if (!Database.IsValidTimePeriod(timespan.Value)) return null;

            var ticks = Database.GetTimespan(timespan.Value);

            Console.WriteLine("TICKS " + ticks);

            var currentDate = new DateTimeOffset(DateTime.UtcNow.Date);
            Console.WriteLine("Current date: " + currentDate.ToString("yyyyMMdd"));
            Console.WriteLine("UTC MIDNIGHT timestamp: " + currentDate.ToUnixTimeMilliseconds());

            var droneTimespans = await Database.GetByTimespan(currentDate.ToString("yyyyMMdd"), supportedDroneNames, timespan.Value);

            while (ticks < currentDate.Ticks)
            {
                currentDate = currentDate.AddDays(-1);
                Console.WriteLine("FROM " + currentDate.ToString("yyyyMMdd"));
                var previousData = await Database.GetByTimespan(currentDate.ToString("yyyyMMdd"), supportedDroneNames, timespan.Value);
                droneTimespans.AddRange(previousData);
            }
            Console.WriteLine($"Earliest date added: " + currentDate.ToString("yyyyMMdd"));
            Console.WriteLine($"Number of points ({timespan.Value}): {droneTimespans.Count}");

            var dronesJson = JsonConvert.SerializeObject(droneTimespans);
            return CreateApiResponse(dronesJson);
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
