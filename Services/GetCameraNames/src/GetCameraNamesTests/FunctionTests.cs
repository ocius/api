using GetCameraNames;
using System.Collections.Generic;
using Xunit;

namespace GetCameraNamesTests
{
    public class FunctionTests
    {
        [Fact]
        public void CreateSuccessResult_WhenValidInput_ShouldCreateSuccessString()
        {
            //Arrange
            var function = new Function();
            var cameraA = new DroneCamera { Id = "1", Name = "Bob", Cameras = new List<string> { "foo", "bar" } };
            var cameraB = new DroneCamera { Id = "2", Name = "Bruce", Cameras = new List<string> { "hello", "world" } };
            var cameras = new List<DroneCamera> { cameraA, cameraB };

            var expected = "Bob has Id 1 and cameras foo bar Bruce has Id 2 and cameras hello world";

            //Act
            var actual = function.CreateSuccessResult(cameras);

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}


