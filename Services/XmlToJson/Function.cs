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
            var date = DateTime.UtcNow.Date.ToShortDateString();
            var drones = await Drones.GetDrones();

            return await SaveDrones(drones, date);
        }

        private async Task<string> SaveDrones(Drones allDrones, string date)
        {
            var response = new List<string>();

            foreach (var data in allDrones.Data)
            {
                var drone = Drone.Create(data, allDrones.Names);

                if (!supportedDrones.Contains(drone.Name)) continue;

                var time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                var result = await Database.InsertDrone(drone, date, time);

                response.Add($"{drone.Name} saved on {date} at {time}: {result}. ");
            }

            return string.Join("", response);
        }
    }
}
