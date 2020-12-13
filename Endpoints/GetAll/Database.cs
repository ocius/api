using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ociusApi.Models;

namespace ociusApi
{
    public static class Database
    {
        private static readonly AmazonDynamoDBClient client = new AmazonDynamoDBClient();
       
        public async static Task<List<string>> GetSupportedDrones()
        {
            ScanRequest supportedDronesScanRequest = Query.CreateSupportedDronesRequest();
            var response = await client.ScanAsync(supportedDronesScanRequest);
            return Query.parseSupportedDroneResponse(response);
        }

        public async static Task<QueryResponse> GetLatestDeprecated(string resource)
        {
            var latestDronesRequest = Query.CreateLatestDroneRequestDeprecated(resource);
            return await client.QueryAsync(latestDronesRequest);; 
        }

        public async static Task<List<DroneSensor>> GetLatest(string resource, List<string> supportedDroneNames)
        {            
            var drones = new List<DroneSensor>();
            foreach (var droneName in supportedDroneNames)
            {
                var latestDronesRequest = Query.CreateLatestDronesRequest(resource, droneName);
                var databaseResponse = await client.QueryAsync(latestDronesRequest);
                var drone = Query.ParseLatestDroneRequest(databaseResponse);
                drones.Add(drone);
            }
            return drones; 
        }

        public async static Task<QueryResponse> GetByTimespan(string date, string timePeriod, string resource)
        {
            if (!IsValidTimePeriod(timePeriod)) return new QueryResponse();

            var timeSpan = GetTimespan(timePeriod);

            var dronesByTimespanRequest = Query.CreateDroneByTimeRequest(date, timeSpan, resource);

            return await client.QueryAsync(dronesByTimespanRequest);
        }

        public static bool IsValidTimePeriod(string timespan)
        {
            var validTimespans = new List<string> { "minute", "hour", "day" };

            return (validTimespans.Contains(timespan));
        }

        public static long GetTimespan(string timeSpan)
        {
            var oneMinuteMilliseconds = 60000;
            var oneHourMilliseconds = 3600000;
            var oneDayMilliseconds = 86400000;

            if(timeSpan == "day")
                return GetByTime(oneDayMilliseconds);

            if (timeSpan == "hour")
                return GetByTime(oneHourMilliseconds);

            return GetByTime(oneMinuteMilliseconds);
        }

        private static long GetByTime(int milliseconds)
        {
            var currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            return currentTimestamp - milliseconds;
        }
    }
}
