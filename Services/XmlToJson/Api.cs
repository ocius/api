using System.Net.Http;
using System.Threading.Tasks;

namespace XmlToJson
{
    public static class Api
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task<string> GetXml(string endpoint)
        {
            return await httpClient.GetStringAsync(endpoint);
        }
    }
}
