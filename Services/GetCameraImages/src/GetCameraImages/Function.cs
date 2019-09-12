using System.Net.Http;
using System.Threading.Tasks;
using Amazon.Lambda.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace GetCameraImages
{
    public class Function
    {
        public async Task FunctionHandler()
        {
            //scrape image

            var client = new HttpClient();
            var response = await client.GetAsync("https://tomamy.co/amytom.jpg");
            var bytes = await response.Content.ReadAsByteArrayAsync();

            //save to s3

            PutObjectRequest request = new PutObjectRequest();
            request.BucketName = "mybucket"
            request.ContentType = contentType;
            request.Key = key;
            request.InputStream = file.InputStream;
            s3Client.PutObject(request);
            //get url
            //add to a DB
        }
    }
}
