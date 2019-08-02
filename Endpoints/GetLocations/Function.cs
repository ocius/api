using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Newtonsoft.Json.Linq;

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
            Console.WriteLine($"================REQUEST: {request}");

            var queryString = request["queryStringParameters"];

            Console.WriteLine($"=================QUERYSRRING: {queryString}");

            string databaseResponse;

            if (queryString is null) //Get latest
            {
                databaseResponse = await Database.GetLatest();
                return CreateResponse(databaseResponse);
            }

            var droneRequest = queryString.ToObject<DroneRequest>();

            Console.WriteLine($"==============TIMESPAN: {droneRequest.Timespan}");

            databaseResponse = await Database.GetByTimespan(droneRequest.Timespan);

            return CreateResponse(databaseResponse);
        }

        private ApiResponse CreateResponse(string body)
        {
            var headers = new Dictionary<string, string>() { { "Access-Control-Allow-Origin", "*" } };
            return new ApiResponse(200, body, headers);
        }
    }
}
