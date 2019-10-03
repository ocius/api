using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GetCameraImages
{
    public class S3
    {
        public static async Task<string> SaveCameraImage(string drone, string camera, long timestamp)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            Console.WriteLine("ATTEMPT TO DOWNLOAD " + drone + " and " + camera);

            var image = await DroneImage.Download(drone, camera);

            stopWatch.Stop();
            var elapsed = stopWatch.Elapsed.TotalSeconds.ToString();
            Console.WriteLine("download took: " + elapsed);

            if (!image.HasData) return $"{Constants.ErrorPrefix} {image.Url}";

            Console.WriteLine("DOWNLOADED " + drone + " and " + camera);

            return await DroneImage.Upload(image.Data, drone, camera, timestamp.ToString());
        }
    }
}
