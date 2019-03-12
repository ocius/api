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
            var input = SampleInputData.Json;
            var expected = GetExpectedJson();

            //Act
            var actual = Function.TransformData(input);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MapIdToName_ShouldCreateCorrectDictionary()
        {
            //Arrange
            var input = SampleInputData.Xml;
            var expected = GetExpectedXml();

            //Act
            var actual = Function.MapIdToName(input);

            //Assert
            Assert.Equal(expected, actual);
        }

        public static string GetExpectedJson()
        {
            return @"[{""Lat"":""-339059283"",""Lon"":""1512347900""},{""Lat"":""-339060231"",""Lon"":""1512347970""},{""Lat"":""-355556288"",""Lon"":""1503833215""},{""Lat"":null,""Lon"":null}]";
        }

        public static IEnumerable<IdToNameMapping> GetExpectedXml()
        {
            var bruce = new IdToNameMapping("2", "Bruce");
            var dory = new IdToNameMapping("3" ,"Dory");
            var bob = new IdToNameMapping("4", "Bob");
            return new List<IdToNameMapping>{bruce, dory, bob};
        }
    }
}
