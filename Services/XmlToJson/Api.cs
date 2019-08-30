using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace XmlToJson
{
    public static class Api
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly string robotsEndpoint = "https://dev.ocius.com.au/usvna/oc_server?listrobots&nodeflate";
        private static readonly string statusEndpoint = "https://dev.ocius.com.au/usvna/oc_server?mavstatus&nodeflate";

        public static async Task<string> GetXml(string endpoint)
        {
            return await httpClient.GetStringAsync(endpoint);
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

        #region Private methods

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
