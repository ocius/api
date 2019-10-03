using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace GetCameraImages
{
    public class DroneCamera
    {
        public string DroneId { get; set; }
        public string DroneName { get; set; }
        public string CameraName { get; set; }
    }

    public class Function
    {
        private static JToken GetDroneJson(string rawData)
        {
            var json = JsonConvert.DeserializeObject(rawData) as JObject;
            var response = json["Response"];
            return response["Camera"];
        }

        private static async Task<Dictionary<string, string>> GetDroneNames()
        {
            var namesEndpoint = "https://dev.ocius.com.au/usvna/oc_server?listrobots&nodeflate";
            var droneNames = await Xml.Get(namesEndpoint);
            var nameJson = Xml.ToJson(droneNames);
            return MapIdToName(nameJson);
        }

        private static Dictionary<string, string> MapIdToName(string nameJson)
        {
            var data = JsonConvert.DeserializeObject(nameJson) as JObject;
            var response = data["Response"];
            var robots = response["robot"];
            var result = new Dictionary<string, string>();

            foreach (var robot in robots)
            {
                var id = robot["sysid"].ToString();
                var name = robot["robotid"].ToString();
                result.Add(id, name);
            }

            return result;
        }


        private static DroneCamera GetDroneCamera(JToken data, IDictionary<string, string> droneNames)
        {
            var foo = data["Name"]?.ToString();

            Console.WriteLine(foo);

            var bar = foo.Split('_').ToList() ;

            //foo_bar
            var id = bar.First();
            var cameraName = bar.Last();

            if (id is null || !droneNames.Keys.Contains(id)) return null;

            var name = droneNames[id];

            return new DroneCamera { DroneId = id, DroneName = name, CameraName = cameraName };

        }

        public async Task<List<string>> FunctionHandler()
        {

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var date = DateTime.UtcNow.Date.ToShortDateString();

            var xml = Xml.Get("https://usvna.ocius.com.au/usvna/oc_server?listcameranames&nodeflate");
            var names = GetDroneNames();

            await Task.WhenAll(xml, names);

            var json = Xml.ToJson(xml.Result);

            var data = GetDroneJson(json);
            var result = new List<string>();

            stopwatch.Stop();
            var elapsed = stopwatch.Elapsed;

            Console.WriteLine("Got camera names in " + elapsed.TotalSeconds.ToString());

            var droneCameras = new List<DroneCamera>();

            foreach (var drone in data)
            {
                var droneCamera = GetDroneCamera(drone, names.Result);
                droneCameras.Add(droneCamera);
            }

            //This code does not download the images in parallel because when performance was measured, it was actually slower
            //The server was overloaded by parallel calls, and total response time was much slower

            foreach (var droneCamera in droneCameras)
            {
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var urls = new List<string>();

                Console.WriteLine("About to save");

                var url = await S3.SaveCameraImage(droneCamera.DroneId, droneCamera.CameraName, timestamp);

                Console.WriteLine("URL " + url);

                urls.Add(url);

                var databaseResponse = await Database.InsertCameraUrls(date, timestamp, droneCamera.DroneName, urls);
                result.AddRange(databaseResponse);
            }

            return result;
        }
    }
}
