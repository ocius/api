using System.Collections.Generic;
using System.Linq;
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
        public async Task<string> FunctionHandler()
        {
            var droneCameras = await GetDroneCameras();
            
            var isSuccess = await SaveToDatabase(droneCameras);

            return isSuccess ? CreateSuccessResult(droneCameras) : "Failed so save drones";
        }

        public string CreateSuccessResult(IEnumerable<DroneCamera> droneCameras)
        {
            var result = droneCameras.Select(drone => $"{drone.Name} has Id {drone.Id} and cameras {string.Join(' ', drone.Cameras)}");

            return string.Join(' ', result);
        }

        public async Task<IEnumerable<DroneCamera>> GetDroneCameras()
        {
            var droneNames = await GetDroneNames();

            var result = new List<DroneCamera>();

            foreach (var drone in droneNames)
            {
                var cameras = await GetCameraNames(drone.Id);

                var bob = new DroneCamera { Id = drone.Id, Name = drone.Name, Cameras = drone.Cameras };
            }

            return result;
        }

        private async Task<bool> SaveToDatabase(IEnumerable<DroneCamera> drones)
        {
            return false;
        }

        private static async Task<IEnumerable<Drone>> GetDroneNames()
        {
            var namesEndpoint = "https://dev.ocius.com.au/usvna/oc_server?listrobots&nodeflate";
            var droneNames = await Api.GetXml(namesEndpoint);
            var nameJson = Json.FromXml(droneNames);
            return MapIdToName(nameJson);
        }

        private static IEnumerable<Drone> MapIdToName(string nameJson)
        {
            var data = JsonConvert.DeserializeObject(nameJson) as JObject;
            var response = data["Response"];
            var robots = response["robot"];
            var result = new List<Drone>();

            foreach (var robot in robots)
            {
                var id = robot["sysid"].ToString();
                var name = robot["robotid"].ToString();
                var drone = new Drone { Id = id, Name = name };
                result.Add(drone);
            }

            return result;
        }

        private static async Task<JToken> GetCameraNames(string id)
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
    }

    public class Drone
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Cameras { get; set; }
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
            //return JsonConvert.SerializeXmlNode(doc);
            return "foo";
        }
    }
}
