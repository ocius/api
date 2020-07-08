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
        public string Response { get; set; }
        public string Name { get; set; }
    }

    public class XmlResponse
    {
        public Response Response { get; set; }
    }

    public class Response
    {
        public List<CameraItem> Camera { get; set; }
    }

    public class CameraItem
    {
        public string Response { get; set; }
        public string Name { get; set; }
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

            return MapIdToCameras(droneStatus);
        }

        public static IEnumerable<DroneCamera> MapIdToCameras(string cameraXml)
        {
            var result = new List<DroneCamera>();

            var cameraJson = Json.FromXml(cameraXml);

            var response = JsonConvert.DeserializeObject<XmlResponse>(cameraJson);

            foreach(var camera in response.Response.Camera)
            {
                var name = camera.Name.Split('_');
                var drone = new DroneCamera { Id = name.First(), Name = name.Last() };
                result.Add(drone);
            }


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
