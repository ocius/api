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
            var allDrones = await Drones.GetAllDrones();

            var date = DateTime.UtcNow.Date.ToShortDateString();

            return await SaveDrones(allDrones, date);
        }

        private async Task<string> SaveDrones(Drones allDrones, string date)
        {
            var response = new List<string>();

            foreach (var data in allDrones.Data)
            {
                var drone = Drone.Create(data, allDrones.Names);

                if (!supportedDrones.Contains(drone.Name)) continue;

                var droneJson = JsonConvert.SerializeObject(drone);

                var result = await Database.InsertDrone(droneJson, date);

                response.Add($"{drone.Name} saved at {date} {result}. ");
            }

            return string.Join("", response);
        }
    }
}
