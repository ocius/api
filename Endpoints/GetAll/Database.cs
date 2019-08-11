using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Threading.Tasks;

namespace ociusApi
{
    public static class Database
    {
        private static readonly AmazonDynamoDBClient client = new AmazonDynamoDBClient();

        public async static Task<QueryResponse> GetLatest()
        {
            var singleDroneRequest = Query.CreateSingleDroneRequest();
            return await client.QueryAsync(singleDroneRequest);
        }

        public async static Task<QueryResponse> GetByTimespan(string timespan)
        {
            if (timespan == "day") return await GetByDay();

            var dateTime = GetTimespan(timespan);
            var dronesByTimespanRequest = Query.CreateDroneByTimeRequest(dateTime);
            return await client.QueryAsync(dronesByTimespanRequest);
        }

        private async static Task<QueryResponse> GetByDay()
        {
            var droneByDayRequest = Query.CreateDroneByDayRequest();
            return await client.QueryAsync(droneByDayRequest);
        }

        private static string GetTimespan(string timeSpan)
        {
            if(timeSpan == "hour")
            {
                var currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                var oneHourAgo = currentTimestamp - 3600;
                Console.WriteLine(oneHourAgo.ToString());
                return oneHourAgo.ToString();
            }

            return "minute";
        }
    }
}
