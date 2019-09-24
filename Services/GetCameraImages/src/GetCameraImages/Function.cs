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
                foreach(var camera in cameras)
                {
                    var path = await SaveImage(date, drone, camera, timestamp);
                    result.Add(path);
                }
            }

            return result;
        }

        private static async Task<string> SaveImage(string date, string drone, string camera, long timestamp)
        {
            var image = await DroneImage.Download(drone, camera);

            if(image.HasData)
            {
                var path = await DroneImage.Upload(image.Data, drone, camera, timestamp.ToString());
                await Database.InsertDrone(date, timestamp, drone, path);
                return path;
            }
            else
            {
                return $"ERROR: Could not download {image.Url}";
            }
        }
    }
}
