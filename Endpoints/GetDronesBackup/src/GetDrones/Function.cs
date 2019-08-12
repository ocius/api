using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Newtonsoft.Json.Linq;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace GetDrones
{
    public class Function
    {
        public async Task<ApiResponse> FunctionHandler(JObject request)
        {
            var queryString = request["queryStringParameters"];
            var resource = request["resource"].ToString();

            var table = resource.Contains("location") ? "DroneLocations" : "DroneSensors"; //move this inside DB

            Console.WriteLine(resource);
            Console.WriteLine(table);

            return queryString.HasValues
                ? await ApiResponse.GetByTimespan(queryString, table)
                : await ApiResponse.GetLatest(table);
        }
    }
}
