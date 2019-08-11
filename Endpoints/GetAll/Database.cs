using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Threading.Tasks;

namespace ociusApi
{
    public static class Database
    {
        private static readonly AmazonDynamoDBClient client = new AmazonDynamoDBClient();

        public async static Task<QueryResponse> GetLatest(string resource)
        {
            Console.WriteLine("============ database");

            var singleDroneRequest = Query.CreateSingleDroneRequest(resource);
            return await client.QueryAsync(singleDroneRequest);
        }

        public async static Task<QueryResponse> GetByTimespan(string timespan, string resource)
        {
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
                var currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                var oneHourAgo = currentTimestamp - 3600;
                Console.WriteLine(oneHourAgo.ToString());
                return oneHourAgo.ToString();
            }

            return "minute";
        }
    }
}
