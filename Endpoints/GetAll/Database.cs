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
            var brucerequest = Query.CreateSingleDroneRequest(resource, "Bruce");

            var bobTask = client.QueryAsync(bobRequest);
            var bruceTask = client.QueryAsync(brucerequest);

            var response = new List<Task<QueryResponse>> { bobTask, bruceTask };

            QueryResponse[] foo = await Task.WhenAll(response);

            var result = new QueryResponse();

            foreach(var bar in foo)
            {
                result.Items.AddRange(bar.Items);
            }

            return result;
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
