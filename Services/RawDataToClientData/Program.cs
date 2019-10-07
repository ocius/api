using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.DynamoDBEvents;
using Newtonsoft.Json;

namespace RawDataToClientData
{
    class Program
    {
        static async Task Main()
        {
            var cameras = await Database.GetCameras();

            foreach (var record in dynamoEvent.Records)
            {
                if (record.EventName != "INSERT") continue;

                var json = Document.FromAttributeMap(record.Dynamodb.NewImage).ToJson();
                var drone = JsonConvert.DeserializeObject<Drone>(json);

                var droneCameras = cameras.ContainsKey(drone.Name) ? cameras[drone.Name] : "";
                var droneLocation = DroneLocation.GetLocation(drone.Name, drone.Data);
                var droneSensors = DroneSensors.GetSensors(drone.Name, drone.Data, droneCameras);

                await Database.InsertAsync(droneLocation, "DroneLocations", drone.Timestamp);
                await Database.InsertAsync(droneSensors, "DroneSensors", drone.Timestamp);
            }
        }
    }
}
