using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Amazon.Lambda.Core;
using Newtonsoft.Json.Linq;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ociusApi
{
    public class DroneRequest
    {
        public string Timespan { get; set; }
    }

    public class Function
    {
        public async Task<ApiResponse> FunctionHandler(JObject request)
        {
            var queryString = request["queryStringParameters"];

            var input = queryString.ToObject<DroneRequest>();

            Console.WriteLine($"TIMESPAN: {input.Timespan}");

            var responseBody = await Database.GetByTimespan(input.Timespan);

            return CreateResponse(responseBody);
        }

        private ApiResponse CreateResponse(string body)
        {
            var headers = new Dictionary<string, string>() { { "Access-Control-Allow-Origin", "*" } };
            return new ApiResponse(200, body, headers);
        }
    }
}
