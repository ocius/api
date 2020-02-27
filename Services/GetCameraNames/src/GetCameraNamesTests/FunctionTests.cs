using GetCameraNames;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var cameraA = new Drone { Id = "1", Name = "Bob", Cameras = new List<string> { "foo", "bar" } };
            var cameraB = new Drone { Id = "2", Name = "Bruce", Cameras = new List<string> { "hello", "world" } };
            var cameras = new List<Drone> { cameraA, cameraB };

            var expected = "Bob has Id 1 and cameras foo bar Bruce has Id 2 and cameras hello world";

            //Act
            var actual = function.CreateSuccessResult(cameras);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetDroneNamesTests()
        {
            //Arrange
            var function = new Function();
            var bobId = "4";

            //Act
            var actual = await function.GetDroneNames();
            var bob = actual.Where(drone => drone.Key == bobId);

            //Assert
            Assert.True(bob.Any());
        }

        [Fact]
        public async Task AddCameraNamesTests()
        {
            //Arrange
            var function = new Function();
            var bobId = "4";

            //Act
            var actual = await function.AddCameraNames();
            var bob = actual.Where(drone => drone.Id == bobId && drone.Name == "masthead");

            //Assert
            Assert.True(bob.Any());
        }

        [Fact]
        public async Task GetDroneCamerasTests()
        {
            //Arrange
            var function = new Function();
            var bobId = "4";
            var bobCamera = "masthead";

            //Act
            var actual = await function.GetDroneCameras();
            var bob = actual.Where(drone => drone.Id == bobId).First();

            //Assert
            Assert.Contains(bobCamera, bob.Cameras);
        }
    }
}


