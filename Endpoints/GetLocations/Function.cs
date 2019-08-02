using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Newtonsoft.Json.Linq;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace GetLocations
{
    public class Function
    {
        public async Task<ApiResponse> FunctionHandler(JObject request)
        {
            var timespan = request["queryStringParameters"];

            return timespan.HasValues 
                ? await Drone.GetLocationsByTimespan(timespan)
                : await Drone.GetLatestLocations();
        }
    }
}
