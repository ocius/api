using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RawDataToClientData.Models;
using System;
using System.Linq;

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
        public string Cameras { get; set; }

        public static string GetSensors(string name, string data, string cameras)
        {
            var json = JsonConvert.DeserializeObject(data) as JObject;

            var mavpos = json["mavpos"];
            var status = mavpos["status"] ?? "Inactive";
            var water_depth = mavpos["water_dep"] ?? "0";
            var water_temp = mavpos["water_tmp"] ?? "0";
            var wind_speed = mavpos["wind_spd"] ?? "0";
            var wind_direction = mavpos["wind_dir"] ?? "0";
            var boat_speed = mavpos["groundspeed"] ?? "0";
            var compass = mavpos["COMPASS_RAW"] ?? "0";
            var lat = compass["lat"] ?? "0";
            var lon = compass["lon"] ?? "0";
            var heading = compass["heading"] ?? "0";


            Console.WriteLine("================== DATA =============");
            Console.WriteLine(data);

            var batteries = JsonConvert.DeserializeObject<Batteries>(data);

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
                Lat = lat.ToString(),
                Lon = lon.ToString(),
                BatteryA = batteries.Tqb.First().Vol.Insert(2, "."),
                BatteryB = batteries.Tqb.First().Vol.Insert(2, "."),
                Cameras = cameras
            };

            return JsonConvert.SerializeObject(sensors);
        }
    }
}