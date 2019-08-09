using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ociusApi
{
    public class Drone
    {
        public string Name { get; set; }
        public string Timestamp { get; set; }
        public string Status { get; set; }
        public string Mode { get; set; }
        public string Sail_mode { get; set; }
        public string Wp_dist { get; set; }
        public string Next_wp { get; set; }
        public string Water_depth { get; set; }
        public string Water_temp { get; set; }
        public string Water_speed { get; set; }
        public string Wind_Speed { get; set; }
        public string Wind_direction { get; set; }
        public string Boat_speed { get; set; }
        public string Throttle { get; set; }
        public string Num_Sats { get; set; }
        public string Hdop { get; set; }
        public string Heading { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }

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

                if (key == "Name") drone.Name = value?.S ?? "";
                if (key == "Timestamp") drone.Timestamp = value?.N ?? "";
                if (key == "Status") drone.Status = value?.S ?? "";
                if (key == "Mode") drone.Mode = value?.S ?? "";
                if (key == "Water_depth") drone.Water_depth = value?.S ?? "";
                if (key == "Water_speed") drone.Water_speed = value?.S ?? "";
                if (key == "Water_temp") drone.Water_temp = value?.S ?? "";
                if (key == "Wind_Speed") drone.Wind_Speed = value?.S ?? "";
                if (key == "Wind_direction") drone.Wind_direction = value?.S ?? "";
                if (key == "Heading") drone.Heading = value?.S ?? "";
                if (key == "Lat") drone.Lat = value?.S ?? "";
                if (key == "Lon") drone.Lon = value?.S ?? "";
            }

            return drone;
        }
    }
}
