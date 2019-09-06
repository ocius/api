using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Newtonsoft.Json.Linq;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ociusApi
{
    public class Function
    {
        public async Task<ApiResponse> FunctionHandler(JObject request)
        {
            var queryString = request["queryStringParameters"];
            var resource = request["resource"].ToString();

            return queryString.HasValues 
                ? await ApiResponse.GetByTimespan(queryString, resource) 
                : await ApiResponse.GetLatest(resource);
        }
    }
}
