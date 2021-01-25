using Amazon.DynamoDBv2.Model;
using ociusApi.Models;
using System.Collections.Generic;

namespace ociusApi
{
    public class DroneLocation
    {
        public string Name { get; set; } = "Unknown Name";
        public string Timestamp { get; private set; } = "0";
        public string Lat { get; private set; } = "0";
        public string Lon { get; private set; } = "0";
        public string WaterTemp { get; private set; } = "0";

        public static DroneLocation CreateDrone(Dictionary<string, AttributeValue> attributes)
        {
            var drone = new DroneLocation();

            foreach (KeyValuePair<string, AttributeValue> kvp in attributes)
            {
                var key = kvp.Key;
                var value = kvp.Value;
                switch(key) 
                {
                    case "Timestamp":   drone.Timestamp = value?.N ?? ""; break;
                    case "Lat":         drone.Lat = value?.S ?? "";       break;
                    case "Lon":         drone.Lon = value?.S ?? "";       break;
                    case "Name":        drone.Name = value?.S ?? "";      break;
                    case "WaterTemp":   drone.WaterTemp = value?.S ?? ""; break;
                }

            }

            return drone;
        }
    }
}