
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace RawDataToClientData.Tests
{
    public class FunctionTest
    {
        private string rawData => GetRawData().Result;

        [Fact]
        public void Foo()
        {
            //Arrange
            var rawData = "foobar";
            var expected = "foo";

            //Act
            var actual = Drone.TransformData(rawData);

            //Assert
            Assert.Equal(expected, actual);
        }

        public static async Task<string> GetRawData()
        {
            var httpClient = new HttpClient();
            const string endpoint = "https://dev.ocius.com.au/usvna/oc_server?mavstatus&nodeflate";
            return await httpClient.GetStringAsync(endpoint);
        }
    }
}
