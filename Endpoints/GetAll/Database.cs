using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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

            var result = "results";

            foreach(var item in response.Items)
            {
                result += GetJson(item);
                result += "##################################################";

            }

            return result;
        }

        private static string GetJson(Dictionary<string, AttributeValue> attributeList)
        {
            var result = "";

            foreach (KeyValuePair<string, AttributeValue> kvp in attributeList)
            {
                var attributeName = kvp.Key;
                var value = kvp.Value;

                result += (
                    attributeName + " " +
                    (value.S == null ? "" : "S=[" + value.S + "]") +
                    (value.N == null ? "" : "N=[" + value.N + "]") +
                    (value.SS == null ? "" : "SS=[" + string.Join(",", value.SS.ToArray()) + "]") +
                    (value.NS == null ? "" : "NS=[" + string.Join(",", value.NS.ToArray()) + "]") +
                    "=============================================================================="
                );
            }

            return result;
        }

        private static string GetQuery(string timeSpan)
        {
            if(timeSpan == "day")
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
