using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Threading.Tasks;

namespace GetCameraImages
{
    public static class Database
    {
        private static readonly AmazonDynamoDBClient client = new AmazonDynamoDBClient();
        private static readonly Table table = Table.LoadTable(client, "CameraImageUrls");

        public async static Task<string> InsertCameraUrls(string date, long timestamp, string drone, string cameras)
        {
            var cameraImageDocument = CreateCameraDocument(date, timestamp, drone, cameras);

            try
            {
                await table.PutItemAsync(cameraImageDocument);
                return "SUCCESS";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to save drone. Exception: " + ex);
                return "ERROR";
            }
        }

        private static Document CreateCameraDocument(string date, long timestamp, string drone, string cameras)
        {
            return new Document
            {
                ["Date"] = date,
                ["Timestamp"] = timestamp,
                ["Name"] = drone, 
                ["Cameras"] = cameras
            };
        }
    }
}
