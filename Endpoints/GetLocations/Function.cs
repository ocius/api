using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace GetLocations
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

            var droneRequest = queryString.ToObject<DroneRequest>();

            Console.WriteLine($"TIMESPAN: {droneRequest.Timespan}");

            var responseBody = await Database.GetByTimespan(droneRequest.Timespan);

            return CreateResponse(responseBody);
        }

        private ApiResponse CreateResponse(string body)
        {
            var headers = new Dictionary<string, string>() { { "Access-Control-Allow-Origin", "*" } };
            return new ApiResponse(200, body, headers);
        }
}
