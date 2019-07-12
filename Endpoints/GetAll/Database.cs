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

        public async static Task<string> GetByTimespan(string timespan)
        {

            var query = GetQuery(timespan);

            var request = new QueryRequest
            {

                TableName = "TimeSeriesDroneData",
                KeyConditionExpression = "#dt = :query",
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    {"#dt", "Date"}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":query", new AttributeValue { S = query } }
                }
            };

            var response = await client.QueryAsync(request);
            return response.Items.Count.ToString();
        }

        private static string GetQuery(string timeSpan)
        {
            if(timeSpan == "hour")
            {
                return DateTime.UtcNow.Date.ToShortDateString();
            }

            return "live";
        }

        //live
        //minute
        //hour
        //day
        //week
        //year
        //all
    }
}
