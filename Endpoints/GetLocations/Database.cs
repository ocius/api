using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Threading.Tasks;

namespace GetLocations
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
            var dateTime = GetDateTime(timespan);
            var dronesByTimespanRequest = Query.CreateDroneByTimespanRequest(dateTime);
            return await client.QueryAsync(dronesByTimespanRequest);
        }

        private static string GetDateTime(string timeSpan)
        {
            if (timeSpan == "day")
            {
                return DateTime.UtcNow.Date.ToShortDateString();
            }

            return "hour";
        }
    }
}
