using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ociusApi
{
    public static class Database
    {
        private static readonly AmazonDynamoDBClient client = new AmazonDynamoDBClient();
       
        public async Task<string> GetSupportedDrones()
        {
            ScanRequest supportedDronesScanRequest = Query.CreateSupportedDronesRequest();
            var response = await client.Scan(supportedDronesScanRequest);
            return Query.parseSupportedDroneResponse(response);
        }

        public async static Task<QueryResponse> GetLatestDeprecated(string resource)
        {
            var latestDronesRequest = Query.CreateLatestDronesRequestDeprecated(resource);
            var databaseResponse = await client.QueryAsync(latestDronesRequest);
            return Query.ParseLatestDroneRequest(databaseResponse); 
        }

        public async static DroneSensor GetLatest(string resource, string droneName)
        {
            var latestDronesRequest = Query.CreateLatestDronesRequest(resource, droneName);
            var databaseResponse = await client.QueryAsync(latestDronesRequest);
            return Query.ParseLatestDroneRequest(databaseResponse); 
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
