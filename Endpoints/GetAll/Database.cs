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

        public async static Task<QueryResponse> GetLatest(string resource)
        {
            var latestDronesRequest = Query.CreateLatestDronesRequest(resource);

            return await client.QueryAsync(latestDronesRequest);
        }

        public async static Task<QueryResponse> GetByTimespan(string timePeriod, string resource)
        {
            if (!IsValidTimePeriod(timePeriod)) return new QueryResponse();

            var timeSpan = GetTimespan(timePeriod);

            var dronesByTimespanRequest = Query.CreateDroneByTimeRequest(timeSpan, resource);

            return await client.QueryAsync(dronesByTimespanRequest);
        }

        private static bool IsValidTimePeriod(string timespan)
        {
            var validTimespans = new List<string> { "minute", "hour", "day" };

            return (validTimespans.Contains(timespan));
        }

        private static string GetTimespan(string timeSpan)
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

        private static string GetByTime(int milliseconds)
        {
            var currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var timePeriod = currentTimestamp - milliseconds;
            return timePeriod.ToString();
        }
    }
}
