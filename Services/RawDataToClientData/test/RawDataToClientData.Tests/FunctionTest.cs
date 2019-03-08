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
            var actual = Drone.TransformData(input);

            //Assert
            Assert.Equal(expected, actual);
        }

        public static string GetExpected()
        {
            var expected = @"{""Lat"":""-339059283"",""Lon"":""1512347900""}";

            return expected.ToString();
        }
    }
}
