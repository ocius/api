using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace XmlToJson
{
    public class Drone
    {
        public string Name { get; private set; }
        public string Data { get; private set; }

        public static Drone Create(JToken droneData, IDictionary<string, string> droneNames)
        {
            var name = GetName(droneData, droneNames);
            return new Drone { Name = name, Data = droneData.ToString() };
        }

        private static string GetName(JToken droneData, IDictionary<string, string> droneNames)
        {
            var data = droneData["mavpos"];
            var id = data["sysid"]?.ToString();

            if (id is null) return "Unknown Drone";

            return droneNames[id];
        }
    }
}
