using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Newtonsoft.Json.Linq;
using Amazon.DynamoDBv2.Model;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ociusApi
{
    public class Function
    {
        public async Task<ApiResponse> FunctionHandler(JObject request)
        {
            var queryString = request["queryStringParameters"];
            return queryString.HasValues ? await GetLocationsByTimespan(queryString) : await GetLatestLocations();
        }

        private static async Task<ApiResponse> GetLocationsByTimespan(JToken queryString)
        {
            var timespan = queryString.ToObject<Timespan>();
            var databaseResponse = await Database.GetByTimespan(timespan.Value);
            return CreateResponse(databaseResponse);
        }

        private static async Task<ApiResponse> GetLatestLocations()
        {
            var databaseResponse = await Database.GetLatest();
            return CreateResponse(databaseResponse);
        }

        private static ApiResponse CreateResponse(QueryResponse databaseResponse)
        {
            var droneJson = Drone.ToJson(databaseResponse);
            return ApiResponse.CreateApiResponse(droneJson);
        }
    }
}
