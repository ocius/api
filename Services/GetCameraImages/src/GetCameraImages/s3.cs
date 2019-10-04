using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GetCameraImages
{
    public class S3
    {
        public static async Task<string> SaveCameraImage(string drone, string camera, long timestamp)
        {
            var downloadStopwatch = new Stopwatch();
            downloadStopwatch.Start();
            var image = await DroneImage.Download(drone, camera);
            downloadStopwatch.Stop();
            var downloadTime = downloadStopwatch.Elapsed.TotalSeconds.ToString();

            Console.WriteLine("image has data " + image.HasData);

            if (!image.HasData) return $"{Constants.ErrorPrefix} {image.Url}";


            var uploadStopwatch = new Stopwatch();
            uploadStopwatch.Start();
            var imagePath = await DroneImage.Upload(image.Data, drone, camera, timestamp.ToString());
            uploadStopwatch.Stop();
            var uploadTime = downloadStopwatch.Elapsed.TotalSeconds.ToString();

            Console.WriteLine( $"Downloaded {imagePath} in {downloadTime} seconds, uploaded in {uploadTime} seconds");
            return imagePath;
        }
    }
}
