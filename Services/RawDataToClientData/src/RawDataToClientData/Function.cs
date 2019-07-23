using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Xml;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace RawDataToClientData
{
    public class Function
    {
        private static readonly AmazonDynamoDBClient client = new AmazonDynamoDBClient();

        public async Task FunctionHandler(JObject input, ILambdaContext context)
        {
            //var rawDataTableName = "RawData";
            //var rawData = Database.ReadAsync(client, rawDataTableName);
            //var xml = Api.GetXmlAsync();
            //await Task.WhenAll(rawData, xml);
            //var mappingBetweenNameAndId = MapIdToName(xml.Result);
            //var cleanData = TransformData(rawData.Result, mappingBetweenNameAndId);

            Console.WriteLine(input.ToString());

            var first = input[0];
            var dynamoDb = first["dynamodb"];
            var item = dynamoDb["NewImage"];
            var name = item["Name"].ToString();
            var data = item["Data"];

            var drone = new Drone(name, "1234", data.ToString());

            var json = JsonConvert.SerializeObject(drone);

            await Database.InsertAsync(client, json);
        }

        

        public static string TransformData(string rawData, Dictionary<string, string> mappingBetweenNameAndId)
        {
            var vehicles = GetVehiclesArray(rawData);
            var drones = vehicles.Select(v => CreateDrone(v, mappingBetweenNameAndId)).ToList();
            var owner = new Owner(drones);
            return JsonConvert.SerializeObject(owner);
        }

        public static Dictionary<string, string> MapIdToName(string xml)
        {
            Console.WriteLine(xml);
            var json = GetJson(xml);
            var data = JsonConvert.DeserializeObject(json) as JObject;
            var response = data["Response"];
            var robots = response["robot"];
            var result = new Dictionary<string, string>();
            foreach(var robot in robots){
                var id = robot["sysid"].ToString();
                var name = robot["robotid"].ToString();
                result.Add(id, name);
            }
            return result;
        }

        private static Drone CreateDrone(JToken vehicle, IDictionary<string, string> mapping){
            var data = vehicle["mavpos"];
            var id = data["sysid"]?.ToString();
            if (id is null) return new Drone("Unknown", "0", "0");
            var name = mapping[id];
            var lat = data["home_lat"] ?? 0;
            var lon = data["home_lon"] ?? 0;
            return new Drone(name, lat.ToString(), lon.ToString());
        }

        private static JToken GetVehiclesArray(string rawData){
            var json = JsonConvert.DeserializeObject(rawData) as JObject;
            var data = json["Response"]["File"];
            return data["Vehicle"];
        }

        private static string GetJson(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return JsonConvert.SerializeXmlNode(doc);
        }
    }
    
    public class IdToNameMapping
    {
        public string Id {get;}
        public string Name {get;}

        public IdToNameMapping(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
    public class Drone
    {
        public string Name {get;}
        public string Lat {get;}
        public string Lon {get;}  //Long is a reserved word     
        public Drone(string name, string lat, string lon) 
        {
            Name = name;
            Lat = lat;
            Lon = lon;
        }
    }

    public class Owner
    {
        public List<Drone> drones {get;}

        public Owner(List<Drone> _drones)
        {
            drones = _drones;
        }
    }

    public static class Api {
        public static async Task<string> GetXmlAsync()
        {
            var httpClient = new HttpClient();
            const string endpoint = "https://dev.ocius.com.au/usvna/oc_server?listrobots&nodeflate";
            return await httpClient.GetStringAsync(endpoint);
        }
    }
      

    public static class Database
    {
        public static async Task<string> ReadAsync(AmazonDynamoDBClient client, string tableName)
        {
            var table = Table.LoadTable(client, tableName);
            var search = table.Query("Bruce", new QueryFilter("timestamp", QueryOperator.GreaterThan, 0));
            var results = await search.GetRemainingAsync();
            return results.Last().ToJson();
        }

        public async static Task<string> InsertAsync(AmazonDynamoDBClient client, string json)
        {
            var table = Table.LoadTable(client, "CleanDroneData");
            var item = Document.FromJson(json);
            item["Date"] = DateTime.UtcNow.Date.ToShortDateString();
            item["Timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var result = await table.PutItemAsync(item);
            return "foo";
            //await table.PutItemAsync(item);
        }
    }
}



