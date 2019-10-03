using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace GetCameraImages
{
    public static class Xml
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task<string> Get(string endpoint)
        {
            return await httpClient.GetStringAsync(endpoint);
        }

        public static string ToJson(string xml)
        {
            var doc = new XmlDocument { XmlResolver = null }; //Setting resolver to null prevents XXE injection
            doc.LoadXml(xml);
            return JsonConvert.SerializeXmlNode(doc);
        }
    }

}
