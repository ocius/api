using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetCameraImages
{
    public static class Database
    {
        private static readonly AmazonDynamoDBClient client = new AmazonDynamoDBClient();
        private static readonly Table table = Table.LoadTable(client, "CameraImageUrls");

        public async static Task<IEnumerable<string>> InsertCameraUrls(string date, long timestamp, string drone, List<string> urls)
        {
            var validUrls = urls.Where(url => !url.StartsWith(Constants.ErrorPrefix)).Select(url => "https://images.ocius.com.au/" + url);
            var value = string.Join(",", validUrls);
            var cameraImageDocument = CreateCameraDocument(date, timestamp, drone, value);

            try
            {
                await table.PutItemAsync(cameraImageDocument);
                return validUrls;
            }
            catch (Exception ex)
            {
                var error = "Failed to save drone. Exception: " + ex;
                Console.WriteLine(error);
                return new List<string> { error };
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
