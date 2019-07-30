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
                var json = Document.FromAttributeMap(record.Dynamodb.NewImage).ToJson();

                var drone = JsonConvert.DeserializeObject<Drone>(json);

                var droneLocation = Drone.GetLocation(drone.Name, drone.Data);

                await Database.InsertAsync(droneLocation);
            }
        }
    }
}



