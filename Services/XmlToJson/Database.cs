using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Threading.Tasks;

namespace XmlToJson
{
    public static class Database
    {
        private static readonly AmazonDynamoDBClient client = new AmazonDynamoDBClient();
        private static readonly Table table = Table.LoadTable(client, "TimeSeriesDroneData");

        public async static Task<string> InsertDrone(string droneData, string date)
        {
            var droneDocument = CreateDroneDocument(droneData, date);

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

        private static Document CreateDroneDocument(string droneData, string date)
        {
            var drone = Document.FromJson(droneData);
            drone["Date"] = date;
            drone["Timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return drone;
        }
    }
}
