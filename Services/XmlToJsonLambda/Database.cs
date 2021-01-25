using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;

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

        public async static Task<List<string>> GetSupportedDrones()
        {
            ScanRequest supportedDronesScanRequest = CreateSupportedDronesRequest();
            var response = await client.ScanAsync(supportedDronesScanRequest);
            return parseSupportDroneResponse(response);
        }

        private static Document CreateDroneDocument(Drone drone, string date, long time)
        {
            var droneJson = JsonConvert.SerializeObject(drone);
            var droneDocument = Document.FromJson(droneJson);
            droneDocument["Date"] = date;
            droneDocument["Timestamp"] = time;
            return droneDocument;
        }      

        private static ScanRequest CreateSupportedDronesRequest()
        {
            return new ScanRequest{
                TableName = "DroneStatus"
            };
        }

        private static List<string> parseSupportDroneResponse(ScanResponse supportedDronesResponse)
        {
            // assumes every drone has a name, this is a valid assumpuption since the name is the partition key
            // If the table is changed, this may not be a valid assumption
            return supportedDronesResponse.Items.Select(item => item["DroneName"].S).ToList();
        }
    }
}
