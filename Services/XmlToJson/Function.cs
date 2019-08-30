using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace XmlToJson
{
    public class Function
    {
        private readonly List<string> supportedDrones = new List<string> { "Bob", "Bruce" };

        public async Task<string> FunctionHandler()
        {
            var droneNames = Api.GetDroneNames();
            var droneData = Api.GetDroneData();
            Task.WaitAll(droneNames, droneData);

            var date = DateTime.UtcNow.Date.ToShortDateString();
            var response = new List<string>();

            foreach (var data in droneData.Result)
            {
                var drone = Drone.Create(data, droneNames.Result);
                Console.WriteLine("drone: " + drone.Name);

                if (!supportedDrones.Contains(drone.Name)) continue;

                var droneJson = JsonConvert.SerializeObject(drone);

                var isSuccess = await Database.InsertDrone(droneJson, date);
                response.Add($"{drone.Name} saved at {date}: {isSuccess.ToString()}. ");
            }

            return string.Join("", response);
        }
    }
}
