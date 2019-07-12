using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Amazon.Lambda.Core;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using Newtonsoft.Json.Linq;
using Amazon.DynamoDBv2.Model;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ociusApi
{
    public class Function
    {
        public async Task<ApiResponse> FunctionHandler(JObject input)
        {
            var timespan = input["timespan"];
            var body = await Database.GetByTimespan(timespan.ToString());
            var headers = new Dictionary<string, string>(){ { "Access-Control-Allow-Origin", "*" } };
            var response = new ApiResponse(200, body, headers);
            return response;
        }
    }

    public static class Database
    {
        public async static Task<string> GetByTimespan(string timespan)
        {
            var client = new AmazonDynamoDBClient();

            var request = new QueryRequest
            {
                TableName = "OciusDroneData",
                KeyConditionExpression = "#foo > :timestamp",
                ExpressionAttributeNames = new Dictionary<String, String> {
                    {"#foo", "timestamp"}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                {":timestamp", new AttributeValue { N = timespan }}}
            };

            var response = await client.QueryAsync(request);
            return response.Items.Count.ToString();
        }
    }

    public class ApiResponse
    {
        public bool isBase64Encoded = false;
        public int statusCode { get; }
        public string body { get; }
        public IDictionary<string, string> headers {get;}

        public ApiResponse(int statusCode, string body, IDictionary<string,string> headers)
        {
            this.statusCode = statusCode;
            this.body = body;
            this.headers = headers;
        }
    }
}
