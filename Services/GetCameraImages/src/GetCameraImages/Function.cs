using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private static readonly HttpClient client = new HttpClient();

        public List<string> FunctionHandler()
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var cameras = new List<string> { "bob%20mast", "bruce%20mast", "bob_360" };

            //This code does not download the images in parallel because when performance was measured, it was actually slower
            //The server was overloaded by parallel calls, and total response time was slower

            return cameras
                    .Select(async camera => await SaveImage(camera, timestamp))
                    .Select(task => task.Result).ToList();
        }

        private static async Task<string> SaveImage(string camera, string timestamp)
        {
            var image = await DownloadImage(camera);

            var path = $"{timestamp}/{camera}.jpg";

            await UploadImage(image, path);

            return $"Image saved to {path}";
        }

        private static TransferUtility CreateTransferUtility()
        {
            var bucketRegion = RegionEndpoint.APSoutheast2;
            var s3Client = new AmazonS3Client(bucketRegion);
            return new TransferUtility(s3Client);
        }

        private static async Task<Stream> DownloadImage(string camera)
        {
            var imageUrl = $"https://usvna.ocius.com.au/usvna/oc_server?getliveimage&camera={camera}";
            var response = await client.GetAsync(imageUrl);
            return await response.Content.ReadAsStreamAsync();
        }

        private static async Task UploadImage(Stream image, string path)
        {
            var bucketName = "ocius-images";

            using (image as FileStream)
            {
                await FileTransferUtility.UploadAsync(image, bucketName, path);
            }
        }
    }
}
