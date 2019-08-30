using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace XmlToJson
{
    public class Drone
    {
        public string Name { get; private set; }
        public string Data { get; private set; }

        public static Drone Create(JToken vehicle, IDictionary<string, string> mapping)
        {
            var data = vehicle["mavpos"];
            var id = data["sysid"]?.ToString();

            if (id is null) return new Drone { Name = "Unknown", Data = vehicle.ToString() };

            var name = mapping[id];
            return new Drone { Name = name, Data = vehicle.ToString() };
        }
    }
}
