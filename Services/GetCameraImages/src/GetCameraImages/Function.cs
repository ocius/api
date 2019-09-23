using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Transfer;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace GetCameraImages
{
    public class Function
    {
        private static TransferUtility FileTransferUtility => CreateTransferUtility();
        private readonly HttpClient client = new HttpClient();

        public async Task<List<string>> FunctionHandler()
        {
            var baseUrl = "https://usvna.ocius.com.au/usvna/oc_server?getliveimage&camera=";
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var supportedDrones = new List<string> { "bob", "bruce" };
            var cameras = new List<string> { "mast" };
            var result = new List<string>();

            //Although this code could download the images in paralle, it does not
            //This is because when performance was measured, it was faster to do in serial
            //The server was overloaded by parallel calls, and took far longer to respond to the requests

            foreach (var drone in supportedDrones)
            {
                foreach(var camera in cameras)
                {
                    var imageUrl = $"{baseUrl}{drone}%20{camera}";
                    var image = await DownloadImage(imageUrl);
                    var path = $"{drone}/{timestamp}/{camera}.jpg";
                    await UploadImage(image, path);
                    result.Add(path);
                }
            }

            return result;
        }

        private static TransferUtility CreateTransferUtility()
        {
            var bucketRegion = RegionEndpoint.APSoutheast2;
            var s3Client = new AmazonS3Client(bucketRegion);
            return new TransferUtility(s3Client);
        }

        private async Task<Stream> DownloadImage(string url)
        {
            var response = await client.GetAsync(url);
            return await response.Content.ReadAsStreamAsync();
        }

        private async Task UploadImage(Stream image, string path)
        {
            var bucketName = "ocius-images";

            using (image as FileStream)
            {
                await FileTransferUtility.UploadAsync(image, bucketName, path);
            }
        }
    }
}
