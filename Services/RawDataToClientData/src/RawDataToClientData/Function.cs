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
        private static Task<string> xml;

        public async Task FunctionHandler(ILambdaContext context)
        {
            var rawDataTableName = "RawData";
            var rawData = Database.ReadAsync(client, rawDataTableName);
            xml = Api.GetXmlAsync();
            await Task.WhenAll(rawData, xml);
            var cleanData = TransformData(rawData.Result);
            await Database.InsertAsync(client, cleanData);
        }

        public static string TransformData(string rawData)
        {
            var vehicles = GetVehiclesArray(rawData);
            var mappingBetweenNameAndId = MapIdToName(xml.Result);
            var drones = vehicles.Select(v => CreateDrone(v)).ToList();
            var owner = new Owner(drones);
            return JsonConvert.SerializeObject(owner);
        }

        public static List<IdToNameMapping> MapIdToName(string xml)
        {
            var json = GetJson(xml);
            var data = JsonConvert.DeserializeObject(json) as JObject;
            var response = data["Response"];
            var robots = response["robot"];
            var result = robots.Select(r => {
                var id = r["sysid"].ToString();
                var name = r["robotid"].ToString();
                return new IdToNameMapping(id, name);
            });
            return result.ToList();
        }

        private static Drone CreateDrone(JToken vehicle){
            var data = vehicle["mavpos"];
            var id = data["sysid"]?.ToString();
            var lat = data["lat"].ToString();
            var lon = data["lon"].ToString();
            return new Drone(id, lat, lon);
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
        public Drone(string id, string lat, string lon) 
        {
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
            const string endpoint = "https://dev.ocius.com.au/usvna/oc_server?listrobots";
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

        public async static Task InsertAsync(AmazonDynamoDBClient client, string json)
        {
            var table = Table.LoadTable(client, "ClientData");
            var item = Document.FromJson(json);
            item["name"] = "Ocius";
            item["timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            await table.PutItemAsync(item);
        }
    }
}



