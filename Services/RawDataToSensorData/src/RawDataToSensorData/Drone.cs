﻿using Newtonsoft.Json;
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
            var compass = mavpos["COMPASS_RAW"];
            var lat = compass["lat"] ?? 0;
            var lon = compass["lon"] ?? 0;
            var heading = compass["heading"] ?? 0;

            var location = new DroneSensors(name, lat.ToString(), lon.ToString(), heading.ToString());

            return JsonConvert.SerializeObject(location);
        }
    }

    public class DroneSensors
    {
        public string Name { get; }
        public string Lat { get; }
        public string Lon { get; }  //Long is a reserved word
        public string Heading { get; }

        public DroneSensors(string name, string lat, string lon, string heading)
        {
            Name = name;
            Lat = lat;
            Lon = lon;
            Heading = heading;
        }
    }
}