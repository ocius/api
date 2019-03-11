using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ociusApi
{
    public class Function
    {
        public async Task<ApiResponse> FunctionHandler(ILambdaContext context)
        {
            var json = await Database.GetAll();
            var response = new ApiResponse(200, json);
            return response;
        }
    }

    public static class Database
    {
        public async static Task<string> GetAll()
        {
            var client = new AmazonDynamoDBClient();
            var table = Table.LoadTable(client, "ClientData");
            var search = table.Query("Bruce", new QueryFilter("timestamp", QueryOperator.GreaterThan, 1550304750));
            var results = await search.GetRemainingAsync();
            return results.Last().ToJson();
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
}
