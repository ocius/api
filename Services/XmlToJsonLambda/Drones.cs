using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XmlToJson
{
    public class Drones
    {
        public JToken Data { get; private set; }
        public Dictionary<string, string> Names { get; private set; }

        public static async Task<Drones> GetDrones()
        {
            var data = await GetDroneData();
            var names = await GetDroneNames();
            return new Drones { Data = data, Names = names };
        }

        #region Private methods

        private static async Task<Dictionary<string, string>> GetDroneNames()
        {
            var namesEndpoint = "https://usvna.ocius.com.au/usvna/oc_server?listrobots&nodeflate";
            var droneNames = await Api.GetXml(namesEndpoint);
            var nameJson = Json.FromXml(droneNames);
            return MapIdToName(nameJson);
        }

        private static async Task<JToken> GetDroneData()
        {
            var dataEndpoint = "https://usvna.ocius.com.au/usvna/oc_server?mavstatus&nodeflate";
            var droneStatus = await Api.GetXml(dataEndpoint);
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

        #endregion
    }
}
