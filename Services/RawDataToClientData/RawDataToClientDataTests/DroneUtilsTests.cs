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

        [Fact]
        public void ParseBattery_WhenBattery_ReturnsVoltage()
        {
            //Arrange
            var battery = new Battery(Id="1", Vol="290", Curr="12", Pwr="34", Pcnt="100");
            var expected = "29.0";

            //Act
            var actual = DroneUtils.ParseVoltage(battery);

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}

