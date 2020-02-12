using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RawDataToClientData
{
    public class DroneLocation
    {
        public string Name { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }  //Long is a reserved word
        public string Heading { get; set; }

        public static string GetLocation(string name, string data)
        {
            var json = JsonConvert.DeserializeObject(data) as JObject;

            var mavpos = json["mavpos"];
            var compass = mavpos["COMPASS_RAW"];
            var lat = mavpos["home_lat"] ?? "0";
            var lon = mavpos["home_lon"] ?? "0";
            var heading = compass["heading"] ?? 0;

            var location = new DroneLocation
            {
                Name = name,
                Lat = lat.ToString(),
                Lon = lon.ToString(),
                Heading = heading.ToString()
            };

            return JsonConvert.SerializeObject(location);
        }
    }
}