using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ociusApi
{
    public class ApiResponse
    {
        public bool isBase64Encoded = false;
        public int statusCode { get; private set; }
        public string body { get; private set; }
        public IDictionary<string, string> headers { get; private set; }

        public static async Task<ApiResponse> GetByTimespan(JToken queryString, string resource)
        {
            var timespan = queryString.ToObject<Timespan>();
            var databaseResponse = await Database.GetByTimespan(timespan.Value, resource);
            return CreateResponse(databaseResponse, resource);
        }

        public static async Task<ApiResponse> GetLatest(string resource)
        {
            Console.WriteLine("============ get latest");
            var databaseResponse = await Database.GetLatest(resource);
            return CreateResponse(databaseResponse, resource);
        }

        private static ApiResponse CreateResponse(QueryResponse databaseResponse, string resource)
        {
            var droneJson = CreateDroneJson(databaseResponse, resource);
            var headers = new Dictionary<string, string>() { { "Access-Control-Allow-Origin", "*" } };
            return new ApiResponse { statusCode = 200, body = droneJson, headers = headers };
        }

        private static string CreateDroneJson(QueryResponse databaseResponse, string resource)
        {
            Console.WriteLine("============ create drone json");
            Console.WriteLine("============ resource", resource);

            return resource.Contains("Location") 
                ? DroneLocation.ToJson(databaseResponse)
                : DroneSensor.ToJson(databaseResponse);
        }
    }
}
