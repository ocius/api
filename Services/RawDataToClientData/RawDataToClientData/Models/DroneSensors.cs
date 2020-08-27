using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RawDataToClientData.Models;
using System.Linq;
using System;

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
        public string Boat_speed { get; set; }
        public string Heading { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public string BatteryA { get; set; }
        public string BatteryB { get; set; }
        public string Batteries { get; set; }
        public string Cameras { get; set; }

        public static string GetSensors(string name, string data, string cameras)
        {
            var batteryData = JsonConvert.DeserializeObject<Batteries>(data);
            var batteries = batteryData.Tqb.Select(battery => DroneUtils.ParseVoltage(battery));
            var json = JsonConvert.DeserializeObject(data) as JObject;
            var mavpos = json["mavpos"] ?? new JObject();
            var status = mavpos["status"] ?? "Inactive";
            var water_depth = mavpos["water_dep"] ?? "0";
            var water_temp = mavpos["water_tmp"] ?? "0";
            var wind_speed = mavpos["wind_spd"] ?? "0";
            var wind_direction = mavpos["wind_dir"] ?? "0";
            var boat_speed = mavpos["groundspeed"] ?? "0";

            var location = DroneLocation.GetLocation(name, data);
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
                Boat_speed = boat_speed.ToString(),
                Heading = heading.ToString(),
                Lat = lat,
                Lon = lon,
                Batteries = String.Join(',', batteries),
                Cameras = cameras
            };
            return JsonConvert.SerializeObject(sensors);
        }
    }
}
