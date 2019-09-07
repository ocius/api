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
            var bobRequest = Query.CreateLatestDronesRequest(resource);

            return await client.QueryAsync(bobRequest);
        }

        public async static Task<QueryResponse> GetByTimespan(string timespan, string resource)
        {
            var validTimespans = new List<string> { "minute", "hour", "day" };

            if (!validTimespans.Contains(timespan)) return new QueryResponse();

            if (timespan == "day") return await GetByDay(resource);

            var dateTime = GetTimespan(timespan);
            var dronesByTimespanRequest = Query.CreateDroneByTimeRequest(dateTime, resource);
            return await client.QueryAsync(dronesByTimespanRequest);
        }

        private async static Task<QueryResponse> GetByDay(string resource)
        {
            var droneByDayRequest = Query.CreateDroneByDayRequest(resource);
            return await client.QueryAsync(droneByDayRequest);
        }

        private static string GetTimespan(string timeSpan)
        {
            if(timeSpan == "hour")
            {
                var currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var oneHourAgo = currentTimestamp - 3600000;
                Console.WriteLine(oneHourAgo.ToString());
                return oneHourAgo.ToString();
            }

            return GetByMinute();
        }

        private static string GetByMinute()
        {
            var currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var oneMinuteAgo = currentTimestamp - 60000;
            Console.WriteLine(oneMinuteAgo.ToString());
            return oneMinuteAgo.ToString();
        }
    }
}
