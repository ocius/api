using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.Json;

namespace GetDrones
{
    public class Function
    {
        public static async Task<ApiResponse> FunctionHandler(JObject request)
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

        private static async Task Main(string[] args)
        {
            // Wrap the FunctionHandler method in a form that LambdaBootstrap can work with.
            using (var handlerWrapper = HandlerWrapper.GetHandlerWrapper((Func<JObject, Task<ApiResponse>>)FunctionHandler, new JsonSerializer()))
            {
                // Instantiate a LambdaBootstrap and run it.
                // It will wait for invocations from AWS Lambda and call
                // the handler function for each one.
                using (var bootstrap = new LambdaBootstrap(handlerWrapper))
                {
                    await bootstrap.RunAsync();
                }
            }
        }
    }
}
