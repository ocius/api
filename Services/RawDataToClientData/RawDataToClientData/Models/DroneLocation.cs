using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RawDataToClientData.Models;
using System.Threading.Tasks;

namespace RawDataToClientData
{
    public class DroneLocation
    {
        public string Name { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public string Heading { get; set; }
        public string WaterTemp { get; set; }
        public bool IsSensitive { get; set; }

        public async static Task<string> GetLocationJson(string name, string data)
        { 
            var location = await GetLocation(name, data);

            return JsonConvert.SerializeObject(location);
        }

        public async static Task<DroneLocation> GetLocation(string name, string data)
        {
            var json = JsonConvert.DeserializeObject(data) as JObject;

            var mavpos = json["mavpos"] ?? new JObject();
            var lat = mavpos["lat"] ?? "0";
            var lon = mavpos["lon"] ?? "0";
            var heading = mavpos["vfr_hdg"] ?? "0";
            var waterTemp = mavpos["water_tmp"] ?? "0";

            var isSensitive = await Database.GetDroneSensitivity(name);

            return new DroneLocation
            {
                Name = name,
                Lat = DroneUtils.ParseDecimal(lat),
                Lon = DroneUtils.ParseDecimal(lon),
                Heading = heading.ToString(),
                WaterTemp = waterTemp.ToString(),
                IsSensitive = isSensitive
            };
        }
    }
}