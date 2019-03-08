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
            var actual = Function.TransformData(input);

            //Assert
            Assert.Equal(expected, actual);
        }

        public static string GetExpected()
        {
            return @"[{""Lat"":""-339059283"",""Lon"":""1512347900""},{""Lat"":""-339060231"",""Lon"":""1512347970""},{""Lat"":""-355556288"",""Lon"":""1503833215""},{""Lat"":null,""Lon"":null}]";
            
        }
    }
}
