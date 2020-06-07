using Newtonsoft.Json.Linq;
using RawDataToClientData.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace RawDataToClientDataTests.Models
{
    public class DroneUtilsTests
    {
        [Fact]
        public void Foo()
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
    }
}

