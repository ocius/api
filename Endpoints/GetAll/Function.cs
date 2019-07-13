using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Amazon.Lambda.Core;
using Newtonsoft.Json.Linq;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ociusApi
{
    public class Function
    {
        public async Task<ApiResponse> FunctionHandler(JObject input)
        {
            var timespan = input["timespan"];
            var body = await Database.GetByTimespan(timespan.ToString());
            Console.WriteLine("%%%%%%%%%% BODY " + body);
            return CreateResponse(body);
        }

        private ApiResponse CreateResponse(string body)
        {
            var headers = new Dictionary<string, string>() { { "Access-Control-Allow-Origin", "*" } };
            return new ApiResponse(200, body, headers);
        }
    }
}
