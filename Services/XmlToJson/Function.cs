using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml;
using Amazon.Lambda.Core;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace XmlToJson
{
    public class Function
    {

        private readonly string robotsEndpoint = "https://dev.ocius.com.au/usvna/oc_server?listrobots&nodeflate";
        private readonly string statusEndpoint = "https://dev.ocius.com.au/usvna/oc_server?mavstatus&nodeflate";

        public async Task FunctionHandler()
        {
            var droneStatus = await Api.GetXml(statusEndpoint);
            var droneNames = await Api.GetXml(robotsEndpoint);

            var statusJson = Json.FromXml(droneStatus);
            var droneArray = GetVehiclesArray(statusJson);

            var nameJson = Json.FromXml(droneNames);
            var idToNameMapping = MapIdToName(nameJson);

            foreach(var droneJson in droneArray)
            {
                var drone = CreateDrone(droneJson, idToNameMapping);
                var json = JsonConvert.SerializeObject(drone);
                await Database.InsertDrone(json, drone.Name);
            }
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

        private static JToken GetVehiclesArray(string rawData)
        {
            var json = JsonConvert.DeserializeObject(rawData) as JObject;
            var data = json["Response"]["File"];
            return data["Vehicle"];
        }

        private static Drone CreateDrone(JToken vehicle, IDictionary<string, string> mapping)
        {
            var data = vehicle["mavpos"];
            var id = data["sysid"]?.ToString();
            if (id is null) return new Drone("Unknown", vehicle.ToString());
            var name = mapping[id];
            return new Drone(name, vehicle.ToString());
        }
    }

    public class Drone
    {
        public string Name { get; }
        public string Status { get; }

        public Drone(string name, string status)
        {
            Name = name;
            Status = status;
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
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return JsonConvert.SerializeXmlNode(doc);
        }
    }

    public static class Database
    {
        public async static Task InsertDrone(string data, string name)
        {
            var client = new AmazonDynamoDBClient();
            var table = Table.LoadTable(client, "OciusDroneData");
            var drone = Document.FromJson(data);
            drone["drone"] = name;
            drone["timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            await table.PutItemAsync(drone);
        }
    }
}
