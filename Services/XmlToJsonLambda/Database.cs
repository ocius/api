using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace XmlToJson
{
    public static class Database
    {
        private static readonly AmazonDynamoDBClient client = new AmazonDynamoDBClient();
        private static readonly Table table = Table.LoadTable(client, "TimeSeriesDroneData");

        public async static Task<string> InsertDrone(Drone drone, string date, long time)
        {
            var droneDocument = CreateDroneDocument(drone, date, time);

            try
            {
                await table.PutItemAsync(droneDocument);
                return "SUCCESS";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to save drone. Exception: " + ex);
                return "ERROR";
            }
        }

        private static Document CreateDroneDocument(Drone drone, string date, long time)
        {
            var droneJson = JsonConvert.SerializeObject(drone);
            var droneDocument = Document.FromJson(droneJson);
            droneDocument["Date"] = date;
            droneDocument["Timestamp"] = time;
            return droneDocument;
        }
    }
}
