using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace XmlToJson
{
    public class Function
    {
        public async Task FunctionHandler()
        {
            var droneNames = Drone.GetDroneNames();
            var droneData = Drone.GetDroneData();
            Task.WaitAll(droneNames, droneData);

            foreach (var data in droneData.Result)
            {
                var drone = Drone.Create(data, droneNames.Result);
                var droneJson = JsonConvert.SerializeObject(drone);
                await Database.InsertDrone(droneJson);
            }
        }
    }
}
