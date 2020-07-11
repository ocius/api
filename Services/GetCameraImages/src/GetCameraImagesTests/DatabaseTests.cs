using GetCameraImages;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GetCameraImagesTests
{
    public class DatabaseTests
    {
        [Fact]
        public async Task GetDroneCameras_WhenOneDrone_DoesNotReturnDuplicates()
        {
            //Arrange
            var expected = 1;

            //Act
            var actual = await Database.GetDroneCameras();

            //Assert
            Assert.Equal(expected, actual.Count(drone => drone.Name == "Bob"));
        }
    }
}
