using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Newtonsoft.Json.Linq;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ociusApi
{
    public class Function
    {
        public async Task<ApiResponse> FunctionHandler(JObject request)
        {
            var queryString = request["queryStringParameters"];

            return queryString.HasValues 
                ? await ApiResponse.GetLocationsByTimespan(queryString) 
                : await ApiResponse.GetLatestLocations();
        }
    }
}
