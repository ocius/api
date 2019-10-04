using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Lambda.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace GetCameraImages
{
    public class DroneCamera
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Cameras { get; set; }
    }

    public class Function
    {
        public async Task<List<string>> FunctionHandler()
        {
            var date = DateTime.UtcNow.Date.ToShortDateString();
            var result = new List<string>();

            var bob = new DroneCamera { Id = "4", Name = "Bob", Cameras = new List<string> { "360", "masthead" } };
            var bruce = new DroneCamera { Id = "2", Name = "Bruce", Cameras = new List<string> { "masthead" } };
            var droneCameras = new List<DroneCamera> { bob, bruce };

            //This code does not download the images in parallel because when performance was measured, it was actually slower
            //The server was overloaded by parallel calls, and total response time was much slower


            Console.WriteLine("starting");

            foreach (var droneCamera in droneCameras)
            {
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var urls = new List<string>();

                foreach (var camera in droneCamera.Cameras)
                {
                    var url = await S3.SaveCameraImage(droneCamera.Id, droneCamera.Name, camera, timestamp);
                    urls.Add(url);
                }

                var databaseResponse = await Database.InsertCameraUrls(date, timestamp, droneCamera.Name, urls);

                Console.WriteLine("database response " + databaseResponse);
                //if it was empty, throw an error

                result.AddRange(urls);
            }

            return result;
        }
    }
}
