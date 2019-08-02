using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace GetLocations
{
    public class Drone
    {
        public string Name { get; set; }
        public string Timestamp { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }

        public static async Task<ApiResponse> GetLatestLocations()
        {
            var databaseResponse = await Database.GetLatest();
            return ApiResponse.CreateApiResponse(databaseResponse);
        }

        public static async Task<ApiResponse> GetLocationsByTimespan(JToken queryString)
        {
            var droneRequest = queryString.ToObject<DroneRequest>();
            var databaseResponse = await Database.GetByTimespan(droneRequest.Timespan);
            return ApiResponse.CreateApiResponse(databaseResponse);
        }
    }

    public class DroneRequest
    {
        public string Timespan { get; set; }
    }
}
