using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ociusApi
{
    public class DroneLocation : Drone, IDrone
    {
        public string Name { get; private set; }
        public string Timestamp { get; private set; }
        public string Lat { get; private set; }
        public string Lon { get; private set; }
        public string Heading { get; private set; }

        public override Drone CreateDrone(Dictionary<string, AttributeValue> attributes)
        {
            var drone = new DroneLocation();

            foreach (KeyValuePair<string, AttributeValue> kvp in attributes)
            {
                var key = kvp.Key;
                var value = kvp.Value;

                if (key == "Timestamp") drone.Timestamp = value?.N ?? "";
                if (key == "Lat") drone.Lat = value?.S ?? "";
                if (key == "Lon") drone.Lon = value?.S ?? "";
                if (key == "Name") drone.Name = value?.S ?? "";
                if (key == "Heading") drone.Heading = value?.S ?? "";
            }

            return drone;
        }
    }
}