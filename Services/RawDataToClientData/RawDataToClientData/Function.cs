using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.DynamoDBEvents;
using Newtonsoft.Json;
using System;
using RawDataToClientData.Repositories;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace RawDataToClientData
{
    public class Function
    {
        public async Task FunctionHandler(DynamoDBEvent dynamoEvent)
        {
            var date = DateTime.UtcNow;
            var cameras = await CameraRepository.GetCamerasByDate(date);

            if (cameras.Count == 0)
            {
                Console.WriteLine($"No Cameras found for {date}");
                Console.WriteLine($"Checking previous day for cameras");
                cameras = await CameraRepository.GetCamerasByDate(date.AddDays(-1));
            }

            Console.WriteLine("Camera count: " + cameras.Count);

            foreach (var record in dynamoEvent.Records)
            {
                if (record.EventName != "INSERT") continue;

                var json = Document.FromAttributeMap(record.Dynamodb.NewImage).ToJson();
                var drone = JsonConvert.DeserializeObject<Drone>(json);

                Console.WriteLine("Drone name: " + drone.Name);

                var cameraExists = cameras.ContainsKey(drone.Name);

                Console.WriteLine("camera exists: " + cameraExists);

                var droneCameras = cameraExists ? cameras[drone.Name] : "";

                Console.WriteLine("Drone cameras " + droneCameras);

                var droneLocation = await DroneLocation.GetLocationJson(drone.Name, drone.Data);
                var droneSensors = await DroneSensors.GetSensors(drone.Name, drone.Data, droneCameras);

                await DroneRepository.InsertDrone(droneLocation, "DroneDataLocations", drone.Timestamp);
                await DroneRepository.InsertDrone(droneSensors, "DroneDataSensors", drone.Timestamp);
            }
        }
    }
}


