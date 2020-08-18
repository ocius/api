using Newtonsoft.Json.Linq;
using RawDataToClientData.Models;
using Xunit;

namespace RawDataToClientDataTests.Models
{
    public class DroneUtilsTests
    {
        [Fact]
        public void ParseDecimal_WhenCoordinate_ReturnsLat()
        {
            //Arrange
            var coord = 1512346666;
            var token = JToken.FromObject(coord);
            var expected = 151.2346666.ToString();

            //Act
            var actual = DroneUtils.ParseDecimal(token);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("290", "29.0")]
        [InlineData("2900", "290.0")]
        [InlineData("29", "2.9")]
        [InlineData("2", ".2")]
        public void ParseBattery_WhenBattery_ReturnsVoltage(string voltage, string expected)
        {
            //Arrange
            var battery = new Battery { Id = "1", Vol = voltage, Cur = "12", Pwr = "34", Pcnt = "100"};

            //Act
            var actual = DroneUtils.ParseVoltage(battery);

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}

