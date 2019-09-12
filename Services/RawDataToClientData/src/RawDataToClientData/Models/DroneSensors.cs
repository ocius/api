using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RawDataToClientData.Models;
using System.Linq;

namespace RawDataToClientData
{
    public class DroneSensors
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string Mode { get; set; }
        public string Sail_mode { get; set; }
        public string Wp_dist { get; set; }
        public string Next_wp { get; set; }
        public string Water_depth { get; set; }
        public string Water_temp { get; set; }
        public string Water_speed { get; set; }
        public string Wind_Speed { get; set; }
        public string Wind_direction { get; set; }
        public string Boat_speed { get; set; }
        public string Throttle { get; set; }
        public string Num_Sats { get; set; }
        public string Hdop { get; set; }
        public string Heading { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public string BatteryA { get; set; }
        public string BatteryB { get; set; }

        public static string GetSensors(string name, string data)
        {
            var json = JsonConvert.DeserializeObject(data) as JObject;

            var mavpos = json["mavpos"];
            var status = mavpos["status"] ?? "";
            var mode = mavpos["mode"] ?? "";
            var water_depth = mavpos["water_dep"] ?? "";
            var water_temp = mavpos["water_tmp"] ?? "";
            var water_speed = mavpos["water_spd"] ?? "";
            var wind_speed = mavpos["wind_spd"] ?? "";
            var wind_direction = mavpos["wind_dir"] ?? "";
            var compass = mavpos["COMPASS_RAW"] ?? "";
            var lat = compass["lat"] ?? "";
            var lon = compass["lon"] ?? "";
            var heading = compass["heading"] ?? "";
            var batteries = JsonConvert.DeserializeObject<Batteries>(data);

            var sensors = new DroneSensors
            {
                Name = name,
                Status = status.ToString(),
                Mode = mode.ToString(),
                Water_depth = water_depth.ToString(),
                Water_temp = water_temp.ToString(),
                Water_speed = water_speed.ToString(),
                Wind_Speed = wind_speed.ToString(),
                Wind_direction = wind_direction.ToString(),
                Heading = heading.ToString(),
                Lat = lat.ToString(),
                Lon = lon.ToString(),
                BatteryA = batteries.Tqb.First().Vol.Insert(2, "."),
                BatteryB = batteries.Tqb.First().Vol.Insert(2, ".")
            };

            return JsonConvert.SerializeObject(sensors);
        }
    }
}
