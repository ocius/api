using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Threading.Tasks;

namespace XmlToJson
{
    public static class Database
    {
        public async static Task InsertDrone(string droneData)
        {
            var client = new AmazonDynamoDBClient();
            var table = Table.LoadTable(client, "OciusDroneData");
            var drone = Document.FromJson(droneData);
            drone["date"] = DateTime.UtcNow.Date;
            drone["timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            await table.PutItemAsync(drone);
        }
    }
}
