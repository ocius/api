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
            var cameraA = new Drone { Id = "1", Name = "Bob", Cameras = "foo bar" };
            var cameraB = new Drone { Id = "2", Name = "Bruce", Cameras = "hello world" };
            var cameras = new List<Drone> { cameraA, cameraB };

            var expected = "Bob has Id 1 and cameras foo bar Bruce has Id 2 and cameras hello world";

            //Act
            var actual = function.CreateSuccessResult(cameras);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MapIdToCameras_WhenOneDrone_ReturnsCorrectOutput()
        {
            //Arrange
            var droneCamera = new DroneCamera { Id = "4", Name = "masthead" };
            var expected = new List<DroneCamera> { droneCamera };
            var input = @"<?xml version=""1.0"" encoding=""UTF - 8"" ?><Response><Status>Succeeded</Status><USVName>Ocius USV Server</USVName><Camera><Name>4_masthead</Name><CameraType>None</CameraType></Camera><ResponseTime>0</ResponseTime></Response>";

            //Act
            var actual = Function.MapIdToCameras(input);

            //Assert
            Assert.Equal(expected.First().Name, actual.First().Name);
            Assert.Equal(expected.First().Id, actual.First().Id);
        }

        [Fact]
        public void MapIdToCameras_WhenManyDrones_ReturnsCorrectOutput()
        {
            //Arrange
            var bobCamera = new DroneCamera { Id = "4", Name = "masthead" };
            var bruceCamera = new DroneCamera { Id = "2", Name = "masthead" };
            var expected = new List<DroneCamera> { bobCamera, bruceCamera };


            var input = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><Response><Status>Succeeded</Status><USVName>Ocius USV Server</USVName><Camera><Name>4_masthead</Name><CameraType>None</CameraType></Camera><Camera><Name>2_masthead</Name><CameraType>None</CameraType></Camera><ResponseTime>0</ResponseTime></Response>";

            //Act
            var actual = Function.MapIdToCameras(input);

            //Assert
            Assert.Equal(expected.First().Name, actual.First().Name);
            Assert.Equal(expected.First().Id, actual.First().Id);
            Assert.Equal(expected.Last().Name, actual.Last().Name);
            Assert.Equal(expected.Last().Id, actual.Last().Id);
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

        [Fact]
        public async Task Save()
        {
            //Arrange
            var expected = "Success";
            var function = new Function();
            var drone = new Drone { Id = "3", Name = "Tom", Cameras = "Some camera" };
            var drones = new List<Drone> { drone };

            //Act
            var actual = await function.SaveToDatabase(drones);

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}


