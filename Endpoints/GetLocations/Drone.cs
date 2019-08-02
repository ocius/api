using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace GetLocations
{
    public class Drone
    {
        public string Name { get; private set; }
        public string Timestamp { get; private set; }
        public string Lat { get; private set; }
        public string Lon { get; private set; }

        public static string ToJson(QueryResponse queryResponse)
        {
            if (!IsValidResponse(queryResponse)) return "There were no results for that time range";

            var drones = queryResponse.Items.Select(item => CreateDrone(item));

            return JsonConvert.SerializeObject(drones);
        }

        private static bool IsValidResponse(QueryResponse queryResponse)
        {
            return queryResponse != null && queryResponse.Items != null && queryResponse.Items.Any();
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
    }
}
