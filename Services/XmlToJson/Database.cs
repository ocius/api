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

        public async static Task<bool> InsertDrone(string droneData, string date)
        {
            if (string.IsNullOrEmpty(droneData)) return false;

            var droneDocument = CreateDroneDocument(droneData, date);
            var result = await table.PutItemAsync(droneDocument);
            Console.WriteLine("Result: " + result);
            return result.Values.Count > 0; //Ensure a record was inserted
        }

        private static Document CreateDroneDocument(string droneData, string date)
        {
            Console.WriteLine("droneData: "+ droneData.Substring(0,10));

            var droneDocument = Document.FromJson(droneData);
            droneDocument["Date"] = date;
            droneDocument["Timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            Console.WriteLine("dronedoc: " + droneDocument.Values.Count.ToString());
            return droneDocument;
        }
    }
}
