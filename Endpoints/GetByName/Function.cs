using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json.Linq;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace GetByName
{
    public class Function
    {
        public async Task<ApiResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var name = request.Path.RemoveApiResource();
            var json = await Database.GetByName(name);
            return new ApiResponse(200, json);
        }
    }

    public static class ExtensionMethods
    {
        public static string RemoveApiResource(this string path)
        {
            return path.Substring(path.LastIndexOf('/') + 1);
        }
    }

    public static class Database
    {
        //Todo:
        //drones/{name}/raw Gets data from the raw table
        //Add query string ?history=hour/day/week/month gets all for duration
        //default is 24 hours

        public async static Task<string> GetByName(string name)
        {
            var drone = JObject.Parse(@"{
                ""drone"": ""bruce"", 
                ""lat"": ""1"", 
                ""long"": ""1""
            }");

            return drone.ToString();

            /*
            var client = new AmazonDynamoDBClient();
            var table = Table.LoadTable(client, Constants.Drones);
            var query = table.Query(name, new QueryFilter(Constants.Timestamp, QueryOperator.GreaterThan, duration));
            var results = await query.GetRemainingAsync();
            return results.First().ToJson();

            
            */
        }
    }

    public class ApiResponse
    {
        public bool isBase64Encoded = false;
        public int statusCode { get; }
        public string body { get; }

        public ApiResponse(int responseCode, string json)
        {
            statusCode = responseCode;
            body = json;
        }
    }

    public static class Constants
    {
        public static string Drones => "drones";
        public static string Timestamp => "timestamp";
    }
}
