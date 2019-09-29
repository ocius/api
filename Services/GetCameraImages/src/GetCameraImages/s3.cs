using System.Threading.Tasks;

namespace GetCameraImages
{
    public class S3
    {
        public static async Task<string> SaveCameraImage(string drone, string camera, long timestamp)
        {
            var image = await DroneImage.Download(drone, camera);

            if (!image.HasData) return $"{Constants.ErrorPrefix} {image.Url}";

            return await DroneImage.Upload(image.Data, drone, camera, timestamp.ToString());
        }
    }
}
