using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace GetCameraImages
{
    public class DroneCamera
    {
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


        private static (string, string) GetName(JToken data, IDictionary<string, string> droneNames)
        {
            var foo = data["Name"]?.ToString();

            Console.WriteLine(foo);

            var underscoreIndex = foo.IndexOf('_');

            //foo_bar
            var id = foo.Substring(0, underscoreIndex - 1);
            var cameraName = foo.Substring(underscoreIndex + 1, foo.Length);

            if (id is null || !droneNames.Keys.Contains(id)) return (null, null);

            var name = droneNames[id];

            return (name, cameraName);

        }

        public async Task<List<string>> FunctionHandler()
        {
            var date = DateTime.UtcNow.Date.ToShortDateString();

            var xml = await Xml.Get("https://usvna.ocius.com.au/usvna/oc_server?listcameranames");
            Console.WriteLine(xml);
            var json = Xml.ToJson(xml);
            Console.WriteLine(json);

            var data = GetDroneJson(json);
            var names = await GetDroneNames();
            var result = new List<string>();

            var cameras = new Dictionary<string, List<string>>();

            foreach (var drone in data)
            {
                Console.WriteLine("reached here");

                var (droneName, cameraName) = GetName(drone, names);

                if (cameras.ContainsKey(droneName))
                {
                    cameras[droneName].Add(cameraName);
                }
                else
                {
                    cameras.Add(droneName, new List<string> { cameraName });
                }

                
            }

            //This code does not download the images in parallel because when performance was measured, it was actually slower
            //The server was overloaded by parallel calls, and total response time was much slower

            foreach (var droneCamera in cameras)
            {
                var droneName = droneCamera.Key;
                var droneCameras = droneCamera.Value;
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var urls = new List<string>();

                foreach (var camera in droneCameras)
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
