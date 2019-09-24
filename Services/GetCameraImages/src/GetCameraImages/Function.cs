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
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var drones = new List<string> { "bob", "bruce" };
            var cameras = new List<string> { "%20mast", "_360" };
            var result = new List<string>();

            //This code does not download the images in parallel because when performance was measured, it was actually slower
            //The server was overloaded by parallel calls, and total response time was slower

            foreach (var drone in drones)
            {
                foreach(var camera in cameras)
                {
                    var path = await SaveImage(drone, camera, timestamp);
                    result.Add(path);
                }
            }

            return result;
        }

        private static async Task<string> SaveImage(string drone, string camera, string timestamp)
        {
            var image = await DroneImage.Download(drone, camera);

            return image.HasData
                ? await DroneImage.Upload(image.Data, drone, camera, timestamp)
                : $"ERROR: Could not download {image.Url}";
        }
    }
}
