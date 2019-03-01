
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xunit;

namespace RawDataToClientData.Tests
{
    public class FunctionTest
    {
        private string rawData => GetRawData().Result;

        [Fact]
        public async Task Foo()
        {
            //Arrange
            var rawData = await GetRawData();
            var expected = GetExpected();

            //Act
            var actual = Drone.TransformData(rawData);

            //Assert
            Assert.Equal(expected, actual);
        }

        public static async Task<string> GetRawData()
        {

            var expected = JObject.Parse(@"{
                ""drone"": ""bruce"", 
                ""lat"": ""1"", 
                ""long"": ""1""
            }");

            return expected.ToString();
            var httpClient = new HttpClient();
            const string endpoint = "https://dev.ocius.com.au/usvna/oc_server?mavstatus&nodeflate";
            return await httpClient.GetStringAsync(endpoint);
        }

        public static string GetExpected()
        {
            var expected = JObject.Parse(@"{
                ""drone"": ""bruce"", 
                ""lat"": ""1"", 
                ""long"": ""1""
            }");

            return expected.ToString();
        }
    }
}
