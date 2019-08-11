using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json.Linq;
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

        public static async Task<ApiResponse> GetLocationsByTimespan(JToken queryString)
        {
            var timespan = queryString.ToObject<Timespan>();
            var databaseResponse = await Database.GetByTimespan(timespan.Value);
            return CreateResponse(databaseResponse);
        }

        public static async Task<ApiResponse> GetLatestLocations()
        {
            var databaseResponse = await Database.GetLatest();
            return CreateResponse(databaseResponse);
        }

        private static ApiResponse CreateResponse(QueryResponse databaseResponse)
        {
            var droneJson = Drone.ToJson(databaseResponse);
            var headers = new Dictionary<string, string>() { { "Access-Control-Allow-Origin", "*" } };
            return new ApiResponse { statusCode = 200, body = droneJson, headers = headers };
        }
    }
}
