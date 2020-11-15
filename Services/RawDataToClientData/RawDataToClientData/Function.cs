using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.DynamoDBEvents;
using Newtonsoft.Json;
using System;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace RawDataToClientData
{
    public class Function
    {
        public async Task FunctionHandler(DynamoDBEvent dynamoEvent)
        {
            var cameras = await Database.GetCameras();

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

                var droneLocation = DroneLocation.GetLocationJson(drone.Name, drone.Data);
                var droneSensors = DroneSensors.GetSensors(drone.Name, drone.Data, droneCameras);

                await Database.InsertAsync(droneLocation, "DroneLocations", drone.Timestamp);
                await Database.InsertAsync(droneSensors, "DroneSensors", drone.Timestamp);
            }
        }
    }
}


