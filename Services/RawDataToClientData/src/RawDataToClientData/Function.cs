using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.DynamoDBEvents;
using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace RawDataToClientData
{
    public class Function
    {
        public async Task FunctionHandler(DynamoDBEvent dynamoEvent)
        {
            foreach (var record in dynamoEvent.Records)
            {
                if (record.EventName != "INSERT") continue;

                var json = Document.FromAttributeMap(record.Dynamodb.NewImage).ToJson();
                var drone = JsonConvert.DeserializeObject<Drone>(json);

                var droneLocation = DroneLocation.GetLocation(drone.Name, drone.Data);
                var droneSensors = DroneSensors.GetSensors(drone.Name, drone.Data);

                await Database.InsertAsync(droneLocation, "DroneLocations");
                await Database.InsertAsync(droneSensors, "DroneSensors");
            }
        }
    }
}



