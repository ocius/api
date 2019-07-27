using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.DynamoDBEvents;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Xml;
using System.IO;
using System.Text;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace RawDataToClientData
{
    public class Function
    {

        public async Task FunctionHandler(DynamoDBEvent dynamoEvent)
        {

            foreach (var record in dynamoEvent.Records)
            {
                var json = Document.FromAttributeMap(record.Dynamodb.NewImage).ToJson();

                Console.WriteLine("============= json " + json);

                var drone = JsonConvert.DeserializeObject<Drone>(json);

                Console.WriteLine("============== drone " + drone);

                //get location data
                var droneLocation = GetDroneLocation(drone.Name, drone.Data);

                await Database.InsertAsync(droneLocation);
            }

        }

        public static string GetDroneLocation(string name, string data)
        { 
            var drone = CreateDrone(name, data);
            return JsonConvert.SerializeObject(drone);
        }

        private static DroneLocation CreateDrone(string name, string data)
        {
            var json = JsonConvert.DeserializeObject(data) as JObject;

            Console.WriteLine("===========================json " + json);

            var mavpos = json["mavpos"];

            Console.WriteLine("====================mavpos ", mavpos);

            var lat = mavpos["home_lat"] ?? 0;
            var lon = mavpos["home_lon"] ?? 0;

            return new DroneLocation(name, lat.ToString(), lon.ToString());
        }
    }

    public class Drone
    {
        public string Date { get; set; }
        public string Timestamp { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
    }
    
    public class IdToNameMapping
    {
        public string Id {get;}
        public string Name {get;}

        public IdToNameMapping(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
    public class DroneLocation
    {
        public string Name {get;}
        public string Lat {get;}
        public string Lon {get;}  //Long is a reserved word     
        public DroneLocation(string name, string lat, string lon) 
        {
            Name = name;
            Lat = lat;
            Lon = lon;
        }
    }
}



