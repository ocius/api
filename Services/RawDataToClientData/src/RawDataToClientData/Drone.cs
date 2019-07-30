using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RawDataToClientData
{
    public class Drone
    {
        public string Date { get; set; }
        public string Timestamp { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }

        public static string GetLocation(string name, string data)
        {
            var json = JsonConvert.DeserializeObject(data) as JObject;

            var mavpos = json["mavpos"];
            var lat = mavpos["home_lat"] ?? 0;
            var lon = mavpos["home_lon"] ?? 0;

            var location = new DroneLocation(name, lat.ToString(), lon.ToString());

            return JsonConvert.SerializeObject(location);
        }
    }

    public class DroneLocation
    {
        public string Name { get; }
        public string Lat { get; }
        public string Lon { get; }  //Long is a reserved word
        
        public DroneLocation(string name, string lat, string lon)
        {
            Name = name;
            Lat = lat;
            Lon = lon;
        }
    }
}
