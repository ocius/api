using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RawDataToClientData.Models;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RawDataToClientData.Repositories;

namespace RawDataToClientData
{
    public class DroneSensors
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string Water_depth { get; set; }
        public string Water_temp { get; set; }
        public string Wind_speed { get; set; }
        public string Wind_direction { get; set; }
        public string Current_speed { get; set; }
        public string Current_direction { get; set; }
        public string Boat_speed { get; set; }
        public string Heading { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public string Batteries { get; set; }
        public string BatteryPercentages { get; set; }
        public string Cameras { get; set; }
        public bool IsSensitive { get; set; }

        public async static Task<string> GetSensors(string name, string data, string cameras)
        {
            var json = JsonConvert.DeserializeObject(data) as JObject;
            var mavpos = json["mavpos"] ?? new JObject();
            var status = mavpos["status"] ?? "Inactive";
            var water_depth = mavpos["water_dep"] ?? "0";
            var water_temp = mavpos["water_tmp"] ?? "0";
            var boat_speed = mavpos["groundspeed"] ?? "0";

            var weatherData = mavpos["WEATHER_DATA"] ?? new JObject();
            var wind_speed = weatherData["wind_spd"] ?? "0";
            var wind_direction = weatherData["wind_dir"] ?? "0";

            var waterVelocity = mavpos["WATER_VELOCITY"] ?? new JObject();
            var current_speed = waterVelocity["curr_spd"];
            var current_direction = waterVelocity["curr_dir"];

            var batteryVoltages = new List<string>();
            var batteryPercentages = new List<string>();
            if (json.ContainsKey("tqb"))
            {
                Console.WriteLine($"found tqb for {name}");
                var batteryData = JsonConvert.DeserializeObject<Batteries>(data);
                batteryVoltages = batteryData.Tqb.Select(battery => DroneUtils.ParseVoltage(battery)).ToList();
                batteryPercentages = batteryData.Tqb.Select(battery => battery.Pcnt).ToList();
            }

            var isSensitive = await SensitivityRepository.GetDroneSensitivity(name);

            var location = await DroneLocation.GetLocation(name, data);
            var lat = location.Lat;
            var lon = location.Lon;
            var heading = location.Heading;
            var sensors = new DroneSensors
            {
                Name = name,
                Status = status.ToString(),
                Water_depth = water_depth.ToString(),
                Water_temp = water_temp.ToString(),
                Wind_speed = wind_speed.ToString(),
                Wind_direction = wind_direction.ToString(),
                Current_speed = current_speed.ToString(),
                Current_direction = current_direction.ToString(),
                Boat_speed = boat_speed.ToString(),
                Heading = heading.ToString(),
                Lat = lat,
                Lon = lon,
                Batteries = String.Join(',', batteryVoltages),
                BatteryPercentages = String.Join(',', batteryPercentages),
                Cameras = cameras,
                IsSensitive = isSensitive
            };
            return JsonConvert.SerializeObject(sensors);
        }
    }
}
