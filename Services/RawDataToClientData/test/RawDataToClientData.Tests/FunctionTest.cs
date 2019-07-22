using Xunit;
using System.Collections.Generic;

namespace RawDataToClientData.Tests
{
    public class FunctionTest
    {
        [Fact]
        public void TransformData_ShouldExtractLatLong()
        {
            //Arrange
            var json = SampleInputData.Json;
            var xml = SampleInputData.Xml;
            var mapping = Function.MapIdToName(xml);
            var expected = GetExpectedJson();

            //Act
            var actual = Function.TransformData(json, mapping);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MapIdToName_ShouldCreateCorrectDictionary()
        {
            //Arrange
            var input = SampleInputData.Xml;
            var expected = GetExpectedMapping();

            //Act
            var actual = Function.MapIdToName(input);

            //Assert
            Assert.Equal(expected, actual);
        }

        public static string GetExpectedJson()
        {
            return @"{""drones"":[{""Name"":""Bruce"",""Lat"":""-33.905932"",""Lon"":""151.234788""},{""Name"":""Dory"",""Lat"":""-33.906019"",""Lon"":""151.234799""},{""Name"":""Bob"",""Lat"":""0"",""Lon"":""0""},{""Name"":""Unknown"",""Lat"":""0"",""Lon"":""0""}]}";
        }

        public static Dictionary<string, string> GetExpectedMapping()
        {
            var mapping = new Dictionary<string, string> {
                {"2", "Bruce"}, 
                {"3", "Dory"},
                {"4", "Bob"}
            };
            return mapping;
        }
    }
}
