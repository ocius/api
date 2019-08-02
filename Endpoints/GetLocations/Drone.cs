using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetLocations
{
    public class Drone
    {
        public string Name { get; private set; }
        public string Timestamp { get; private set; }
        public string Lat { get; private set; }
        public string Lon { get; private set; }

        public static async Task<ApiResponse> GetLatestLocations()
        {
            var databaseResponse = await Database.GetLatest();
            var droneJson = CreateDroneJson(databaseResponse);
            return ApiResponse.CreateApiResponse(droneJson);
        }

        public static async Task<ApiResponse> GetLocationsByTimespan(JToken queryString)
        {
            var droneRequest = queryString.ToObject<DroneRequest>();
            var databaseResponse = await Database.GetByTimespan(droneRequest.Timespan);
            var droneJson = CreateDroneJson(databaseResponse);
            return ApiResponse.CreateApiResponse(droneJson);
        }

        private static Drone CreateDrone(Dictionary<string, AttributeValue> attributes)
        {
            var drone = new Drone();

            foreach (KeyValuePair<string, AttributeValue> kvp in attributes)
            {
                var key = kvp.Key;
                var value = kvp.Value;

                if (key == "Timestamp") drone.Timestamp = value?.N ?? "";
                if (key == "Lat") drone.Lat = value?.S ?? "";
                if (key == "Lon") drone.Lon = value?.S ?? "";
                if (key == "Name") drone.Name = value?.S ?? "";
            }

            return drone;
        }

        private static string CreateDroneJson(QueryResponse queryResponse)
        {
            var drones = new List<Drone>();

            if (!IsValidResponse(queryResponse)) return "There were no results for that time range";

            foreach (var item in queryResponse.Items)
            {
                var drone = CreateDrone(item);
                drones.Add(drone);
            }

            return JsonConvert.SerializeObject(drones);
        }

        private static bool IsValidResponse(QueryResponse queryResponse)
        {
            return queryResponse != null &&
                    queryResponse.Items != null &&
                    queryResponse.Items.Any();
        }
    }

    public class DroneRequest
    {
        public string Timespan { get; set; }
    }
}
