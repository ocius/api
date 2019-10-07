using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace XmlToJson
{
    class Program
    {
        private static readonly List<string> supportedDrones = new List<string> { "Bob", "Bruce" };

        static async Task Main()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var date = DateTime.UtcNow.Date.ToShortDateString();
            var drones = await Drones.GetDrones();

            var result = await SaveDrones(drones, date);
            stopwatch.Stop();

            Console.WriteLine(result);
            Console.WriteLine(stopwatch.ElapsedMilliseconds.ToString());
        }

        private static async Task<string> SaveDrones(Drones allDrones, string date)
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
