using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace GetCameraNames
{
    public class DroneCamera
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Cameras { get; set; }
    }

    public class Function
    {
        public async Task<string> FunctionHandler(string input, ILambdaContext context)
        {
            var droneNames = await GetDroneNames();
            var cameras = await GetCameraNames();

            var bob = new DroneCamera { Id = "4", Name = "Bob", Cameras = new List<string> { "360", "masthead" } };

            //save to DB

            return input?.ToUpper();
        }

        private static async Task<JToken> GetCameraNames()
        {
            var dataEndpoint = "https://dev.ocius.com.au/usvna/oc_server?mavstatus&nodeflate";
            var droneStatus = await Api.GetXml(dataEndpoint);
            var statusJson = Json.FromXml(droneStatus);
            return GetDroneJson(statusJson);
        }

        private static JToken GetDroneJson(string rawData)
        {
            var json = JsonConvert.DeserializeObject(rawData) as JObject;
            var data = json["Response"]["File"];
            return data["Vehicle"];
        }

        private static async Task<Dictionary<string, string>> GetDroneNames()
        {
            var namesEndpoint = "https://dev.ocius.com.au/usvna/oc_server?listrobots&nodeflate";
            var droneNames = await Api.GetXml(namesEndpoint);
            var nameJson = Json.FromXml(droneNames);
            return MapIdToName(nameJson);
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

    }

    public static class Api
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task<string> GetXml(string endpoint)
        {
            return await httpClient.GetStringAsync(endpoint);
        }
    }

    public static class Json
    {
        public static string FromXml(string xml)
        {
            var doc = new XmlDocument { XmlResolver = null }; //Setting resolver to null prevents XXE injection
            doc.LoadXml(xml);
            return JsonConvert.SerializeXmlNode(doc);
        }
    }
}
