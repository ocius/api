using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace RawDataToClientData
{
    public class Function
    {
        public async Task FunctionHandler(ILambdaContext context)
        {
            var rawData = ReadRawDataAsync();
            var clientData = TransformData(rawData);
            await Database.InsertAsync(clientData);
        }

        public static string ReadRawDataAsync()
        {
            //here we get the raw data from the db
            //how? it needs to be in there first
            //go fix the other function
            var rawData = @"{
                ""foo"": ""bar"", 
                ""x"": ""y"", 
                ""1"": ""0""
            }";

            return rawData;
        }
        public static string TransformData(string rawData)
        {
            //here is the heavy lifting
            //the logic
            //this part could be great for tdd
            //test by actually calling his api
            //that way we know if he's broken it

            var clientData = @"{
                ""drone"": ""bruce"", 
                ""lat"": ""1"", 
                ""long"": ""1""
            }";

            return clientData;
        }

        public static class Database
        {
            public async static Task InsertAsync(string json)
            {
                var client = new AmazonDynamoDBClient();
                var table = Table.LoadTable(client, "ClientData");
                var item = Document.FromJson(json);
                item["name"] = "Bruce";
                item["timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                await table.PutItemAsync(item);
            }
        }
    }
}
