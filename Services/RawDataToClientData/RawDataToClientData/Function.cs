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
                var isSensitive = await Database.GetDroneSensitivity(drone.Name);
                drone.isSensitive = isSensitive;

                Console.WriteLine("Drone name: " + drone.Name);

                var cameraExists = cameras.ContainsKey(drone.Name);

                Console.WriteLine("camera exists: " + cameraExists);

                var droneCameras = cameraExists ? cameras[drone.Name] : "";

                Console.WriteLine("Drone cameras " + droneCameras);

                var droneLocation = DroneLocation.GetLocationJson(drone.Name, drone.Data, drone.isSensitive);
                var droneSensors = DroneSensors.GetSensors(drone.Name, drone.Data, droneCameras, drone.isSensitive));

                await Database.InsertAsyncWithCompositeKey(droneLocation, "DroneDataLocations", drone.Timestamp);
                await Database.InsertAsyncWithCompositeKey(droneSensors, "DroneDataSensors", drone.Timestamp);
                await Database.InsertAsync(droneLocation, "DroneLocations", drone.Timestamp);
                await Database.InsertAsync(droneSensors, "DroneSensors", drone.Timestamp);
            }
        }
    }
}


