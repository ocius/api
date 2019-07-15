using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Amazon.Lambda.Core;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ociusApi
{
    public class LambdaRequest
    {
        public string Body { get; set; }
    }

    public class DroneRequest
    {
        public string Timespan { get; set; }
    }

    public class Function
    {
        public async Task<ApiResponse> FunctionHandler(JObject request)
        {
            var input = request.ToObject<DroneRequest>();
            var responseBody = await Database.GetByTimespan(input.Timespan);
            return CreateResponse(responseBody);
        }

        private ApiResponse CreateResponse(string body)
        {
            var headers = new Dictionary<string, string>() { { "Access-Control-Allow-Origin", "*" } };
            return new ApiResponse(200, body, headers);
        }
    }
}
