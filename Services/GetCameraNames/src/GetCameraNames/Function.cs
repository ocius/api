using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

/*
 Current state of XML response is: 

    <Response>
        <Status>Succeeded</Status>
        <USVName>Ocius USV Server</USVName>
        <Camera>
            <Name>4_masthead</Name>
            <CameraType>None</CameraType>
        </Camera>
        <ResponseTime>0</ResponseTime>
    </Response>
 */


namespace GetCameraNames
{
    public class DroneCamera
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class CameraResponse
    {
        public string Name { get; set; }
        public string CameraType { get; set; }
    }

    public class Function
    {
        public async Task<string> FunctionHandler()
        {
            var droneCameras = await GetDroneCameras();
            
            var result = await SaveToDatabase(droneCameras);

            return result == "Success" ? CreateSuccessResult(droneCameras) : result;
        }

        public async Task<IEnumerable<Drone>> GetDroneCameras()
        {
            var drones = await GetDroneNames();
            var cameras = await AddCameraNames();
            var result = new List<Drone>();
 
            foreach(var camera in cameras)
            {
                if (drones.ContainsKey(camera.Id))
                {
                    var drone = drones[camera.Id];
                    drone.Cameras += $"{camera.Name},";
                    result.Add(drone);
                }
            }
            return result;
        }

        public async Task<IDictionary<string, Drone>> GetDroneNames()
        {
            var namesEndpoint = "https://dev.ocius.com.au/usvna/oc_server?listrobots&nodeflate";
            var droneNames = await Api.GetXml(namesEndpoint);
            var nameJson = Json.FromXml(droneNames);
            var drones = MapIdToName(nameJson);

            var result = new Dictionary<string, Drone>();

            foreach(var drone in drones)
            {
                drone.Cameras = string.Empty;
                result.Add(drone.Id, drone);
            }

            return result;
        }

        private static IEnumerable<Drone> MapIdToName(string nameJson)
        {
            var data = JsonConvert.DeserializeObject(nameJson) as JObject;
            var response = data["Response"];
            var robots = response["robot"];
            var result = new List<Drone>();

            foreach (var robot in robots)
            {
                var id = robot["sysid"].ToString();
                var name = robot["robotid"].ToString();
                var drone = new Drone { Id = id, Name = name };
                result.Add(drone);
            }

            return result;
        }

        public async Task<IEnumerable<DroneCamera>> AddCameraNames()
        {
            var dataEndpoint = "https://usvna.ocius.com.au/usvna/oc_server?listcameranames&nodeflate";

            var droneStatus = await Api.GetXml(dataEndpoint);
            Console.WriteLine("////////////////////DRONE STATUS///////////////////////");
            Console.WriteLine(droneStatus);

            var statusJson = Json.FromXml(droneStatus);
            Console.WriteLine("////////////////////STATUS JSON///////////////////////");
            Console.WriteLine(statusJson);

            return MapIdToCameras(statusJson);
        }

        public static IEnumerable<DroneCamera> MapIdToCameras(string cameraJson)
        {
            var data = JsonConvert.DeserializeObject(cameraJson) as JObject;

            var response = data["Response"];
            Console.WriteLine("////////////////////response CHILDREN///////////////////////");
            Console.WriteLine(response.Children());

            var cameras = response["Camera"];
            var result = new List<DroneCamera>();

            Console.WriteLine("////////////////////CAMERAS COUNT AND STRING///////////////////////");
            Console.WriteLine(cameras.Count());
            Console.WriteLine(cameras.ToString());


            foreach (var camera in cameras)
            {
                Console.WriteLine("////////////////////SINGLE CAMERA///////////////////////");
                Console.WriteLine(camera.ToString());

                Console.WriteLine("////////////////////TOKEN CHILDREN///////////////////////");
                Console.WriteLine(camera.Children());

                var droneCamera = JsonConvert.DeserializeObject<CameraResponse>(camera.ToString());
                var name = droneCamera.Name.Split('_');
                var drone = new DroneCamera { Id = name.First(), Name = name.Last() };

                Console.WriteLine("////////////////////DRONE NAME///////////////////////");
                Console.WriteLine(drone.Name);

                result.Add(drone);
                Console.WriteLine("////////////////////Result count ///////////////////////");
                Console.WriteLine(result.Count());
            }

            Console.WriteLine("////////////////////END OF LOOP///////////////////////");
            return result;
        }

        public string CreateSuccessResult(IEnumerable<Drone> droneCameras)
        {
            var result = droneCameras.Select(drone => $"{drone.Name} has Id {drone.Id} and cameras {string.Join(' ', drone.Cameras)}");

            return string.Join(' ', result);
        }

        public async Task<string> SaveToDatabase(IEnumerable<Drone> drones)
        {
            

            try
            {
                await Database.InsertAsync(drones);
                return "Success";
            }
            catch (Exception ex)
            {
                var error = "Failed to save drone. Exception: " + ex;
                Console.WriteLine(error);
                return error;
            }
        }
    }

    public class Drone
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Cameras { get; set; }
    }

    public static class Api
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task<string> GetXml(string endpoint)
        {
            return await httpClient.GetStringAsync(endpoint);
        }
    }
}
