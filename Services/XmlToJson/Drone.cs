using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XmlToJson
{
    public class Drone
    {
        private static readonly string robotsEndpoint = "https://dev.ocius.com.au/usvna/oc_server?listrobots&nodeflate";
        private static readonly string statusEndpoint = "https://dev.ocius.com.au/usvna/oc_server?mavstatus&nodeflate";

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

        public static async Task<Dictionary<string, string>> GetDroneNames()
        {
            var droneNames = await Api.GetXml(robotsEndpoint);
            var nameJson = Json.FromXml(droneNames);
            return MapIdToName(nameJson);
        }

        public static async Task<JToken> GetDroneData()
        {
            var droneStatus = await Api.GetXml(statusEndpoint);
            var statusJson = Json.FromXml(droneStatus);
            return GetDroneJson(statusJson);
        }

        private static Dictionary<string, string> MapIdToName(string nameJson)
        {
            var data = JsonConvert.DeserializeObject(nameJson) as JObject;
            var response = data["Response"];
            var robots = response["robot"];
            var result = new Dictionary<string, string>();

            foreach (var robot in robots)
            {
                var id = robot["sysid"].ToString();
                var name = robot["robotid"].ToString();
                result.Add(id, name);
            }

            return result;
        }

        private static JToken GetDroneJson(string rawData)
        {
            var json = JsonConvert.DeserializeObject(rawData) as JObject;
            var data = json["Response"]["File"];
            return data["Vehicle"];
        }
    }
}
