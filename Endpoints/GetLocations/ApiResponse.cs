using System.Collections.Generic;

namespace GetLocations
{
    public class ApiResponse
    {
        public bool isBase64Encoded = false;
        public int statusCode { get; private set; }
        public string body { get; private set; }
        public IDictionary<string, string> headers { get; private set; }

        public static ApiResponse CreateApiResponse(string body)
        {
            var headers = new Dictionary<string, string>() { { "Access-Control-Allow-Origin", "*" } };
            return new ApiResponse { statusCode = 200, body = body, headers = headers };
        }
    }
}
