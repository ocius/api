using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xunit;

namespace RawDataToClientData.Tests
{
    public class FunctionTest
    {
        [Fact]
        public void TransformData_ShouldExtractLatLong()
        {
            //Arrange
            var input = SampleInputData.Json;
            var expected = GetExpected();

            //Act
            var actual = Function.Drone.TransformData(input);

            //Assert
            Assert.Equal(expected, actual);
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
