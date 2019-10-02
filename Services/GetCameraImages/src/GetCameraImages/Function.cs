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
            var date = DateTime.UtcNow.Date.ToShortDateString();
            var droneNames = new List<string> { "Bob", "Bruce" };
            var cameras = new List<string> { "%20mast", "_360" };
            var result = new List<string>();

            //This code does not download the images in parallel because when performance was measured, it was actually slower
            //The server was overloaded by parallel calls, and total response time was much slower

            foreach (var droneName in droneNames)
            {
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var urls = new List<string>();

                foreach (var camera in cameras)
                {
                    var url = await S3.SaveCameraImage(droneName, camera, timestamp);
                    urls.Add(url);
                }
                
                var databaseResponse = await Database.InsertCameraUrls(date, timestamp, droneName, urls);
                result.AddRange(databaseResponse);
            }

            return result;
        }
    }
}
