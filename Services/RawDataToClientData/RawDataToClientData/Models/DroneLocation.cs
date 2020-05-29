using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RawDataToClientData.Models;

namespace RawDataToClientData
{
    public class DroneLocation
    {
        public string Name { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public string Heading { get; set; }

        public static string GetLocation(string name, string data)
        {
            var json = JsonConvert.DeserializeObject(data) as JObject;

            var mavpos = json["mavpos"] ?? new JObject();
            var compass = mavpos["COMPASS_RAW"] ?? new JObject();
            var lat = mavpos["lat"] ?? "0";
            var lon = mavpos["lon"] ?? "0";
            var heading = compass["heading"] ?? "0";

            var location = new DroneLocation
            {
                Name = name,
                Lat = DroneUtils.ParseCoordinates(lat),
                Lon = DroneUtils.ParseCoordinates(lon),
                Heading = heading.ToString()
            };

            return JsonConvert.SerializeObject(location);
        }
    }
}