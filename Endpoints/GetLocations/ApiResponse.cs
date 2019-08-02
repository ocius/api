using System;
using System.Collections.Generic;
using System.Text;

namespace GetLocations
{
    public class ApiResponse
    {
        public bool isBase64Encoded = false;
        public int statusCode { get; }
        public string body { get; }
        public IDictionary<string, string> headers { get; }

        public ApiResponse(int statusCode, string body, IDictionary<string, string> headers)
        {
            this.statusCode = statusCode;
            this.body = body;
            this.headers = headers;
        }
    }
}
