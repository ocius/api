using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ociusApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ociusApi
{
    public class ApiResponse
    {
        #region Properties

        [JsonProperty("isBase64Encoded")]
        public bool IsBase64Encoded = false;

        [JsonProperty("statusCode")]
        public int StatusCode { get; private set; }

        [JsonProperty("body")]
        public string Body { get; private set; }

        [JsonProperty("headers")]
        public IDictionary<string, string> Headers { get; private set; }

        #endregion

        public static async Task<ApiResponse> GetLatest(string resource)
        {
            var databaseResponse = await Database.GetLatest(resource);
            return CreateResponse(databaseResponse, resource);
        }

        public static async Task<ApiResponse> GetByTimespan(JToken queryString, string resource)
        {
            var timespan = queryString.ToObject<Timespan>();
            var databaseResponse = await Database.GetByTimespan(timespan.Value, resource);
            return CreateResponse(databaseResponse, resource);
        }

        #region Private methods

        private static ApiResponse CreateResponse(QueryResponse databaseResponse, string resource)
        {
            var droneJson = CreateDroneJson(databaseResponse, resource);
            var headers = new Dictionary<string, string>() { { "Access-Control-Allow-Origin", "*" } };
            return new ApiResponse { StatusCode = 200, Body = droneJson, Headers = headers };
        }

        private static string CreateDroneJson(QueryResponse databaseResponse, string resource)
        {
            var drone = DroneFactory.CreateDrone(resource);
            return drone.ToJson(databaseResponse);
        }

        #endregion
    }
}
