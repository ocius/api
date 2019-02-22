using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml;
using System.Collections.Generic;
using Amazon.Lambda.Core;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace XmlToJson
{
    public class Function
    {
        public async Task FunctionHandler(ILambdaContext context)
        {
            const string endpoint = "https://dev.ocius.com.au/usvna/oc_server?mavstatus&nodeflate";
            var xml = await Api.GetXml(endpoint);
            var json = Json.FromXml(xml);
            await Database.Insert(json);
        }
    }

    public static class Api
    {
        public static async Task<string> GetXml(string endpoint)
        {
            var httpClient = new HttpClient();
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
        public async static Task Insert(string json)
        {
            var client = new AmazonDynamoDBClient();
            var table = Table.LoadTable(client, "RawData");
            var drone = Document.FromJson(json);
            drone["name"] = "Bruce";
            drone["timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            await table.PutItemAsync(drone);
        }
    }
}
