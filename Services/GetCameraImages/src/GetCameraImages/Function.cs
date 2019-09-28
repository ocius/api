using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Lambda.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace GetCameraImages
{
    public class Function
    {
        public async Task<List<string>> FunctionHandler()
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var date = DateTime.UtcNow.Date.ToShortDateString();
            var drones = new List<string> { "bob", "bruce" };
            var cameras = new List<string> { "%20mast", "_360" };
            var result = new List<string>();

            //This code does not download the images in parallel because when performance was measured, it was actually slower
            //The server was overloaded by parallel calls, and total response time was slower

            foreach (var drone in drones)
            {
                var urls = new List<string>();

                foreach(var camera in cameras)
                {
                    var url = await SaveImage(drone, camera, timestamp);
                    urls.Add(url);
                }

                var value = string.Join(",", urls);
                await Database.InsertCameraUrls(date, timestamp, drone, value);

                result.AddRange(urls);
            }

            return result;
        }

        private static async Task<string> SaveImage(string drone, string camera, long timestamp)
        {
            var image = await DroneImage.Download(drone, camera);

            if(!image.HasData) return $"ERROR: Could not download {image.Url}";

            return await DroneImage.Upload(image.Data, drone, camera, timestamp.ToString());
        }
    }
}
