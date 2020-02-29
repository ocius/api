using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GetCameraNames
{
    public static class Database
    {
        private static readonly AmazonDynamoDBClient client = new AmazonDynamoDBClient();
        private static int Date => int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));
        private static readonly Table table = Table.LoadTable(client, "CameraNames");

        public async static Task InsertAsync(IEnumerable<Drone> drones)
        {
            foreach(var drone in drones)
            {
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var document = CreateDroneDocument(drone, timestamp);
                await table.PutItemAsync(document);
            }
        }

        private static Document CreateDroneDocument(Drone drone, long time)
        {
            var droneJson = JsonConvert.SerializeObject(drone);
            var droneDocument = Document.FromJson(droneJson);
            droneDocument["Date"] = Date;
            droneDocument["Timestamp"] = time;
            return droneDocument;
        }
    }
}
