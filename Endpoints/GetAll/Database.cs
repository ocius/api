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
        private static readonly List<string> droneNames = new List<string> { "Bob", "Bruce" };

        public async static Task<QueryResponse> GetLatest(string resource)
        {
            Console.WriteLine("============ database");

            var bobRequest = Query.CreateSingleDroneRequest(resource, "Bob");
            var bruceRequest = Query.CreateSingleDroneRequest(resource, "Bruce");

            var bobTask = client.QueryAsync(bobRequest);
            var bruceTask = client.QueryAsync(bruceRequest);

            var response = new List<Task<QueryResponse>> { bobTask, bruceTask };

            QueryResponse[] foo = await Task.WhenAll(response);

            var result = new QueryResponse();

            foreach(var bar in foo)
            {
                Console.WriteLine("========= items " + result.Items);

                result.Items.AddRange(bar.Items);
            }

            Console.WriteLine("========= results " + result.Items.Count);

            return result;
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
