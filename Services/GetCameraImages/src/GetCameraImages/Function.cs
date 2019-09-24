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
        private static readonly HttpClient client = new HttpClient();

        public async Task<List<string>> FunctionHandler()
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var supportedDrones = new List<string> { "bob", "bruce" };
            var cameras = new List<string> { "%20mast", "_360" };
            var result = new List<string>();

            //This code does not download the images in parallel because when performance was measured, it was actually slower
            //The server was overloaded by parallel calls, and total response time was slower

            foreach (var drone in supportedDrones)
            {
                foreach(var camera in cameras)
                {
                    var path = await SaveImage(drone, camera, timestamp);
                    result.Add(path);
                }
            }

            return result;
        }

        private static async Task<string> SaveImage(string drone, string camera, string timestamp)
        {
            var baseUrl = "https://usvna.ocius.com.au/usvna/oc_server?getliveimage&camera=";

            var imageUrl = $"{baseUrl}{drone}{camera}";

            var image = await DownloadImage(imageUrl);

            if (image.Length == 0) return $"{imageUrl} was not found";

            var path = $"{drone}/{timestamp}/{camera}.jpg";

            await UploadImage(image, path);

            return $"Image saved to {path}";
        }

        private static TransferUtility CreateTransferUtility()
        {
            var bucketRegion = RegionEndpoint.APSoutheast2;
            var s3Client = new AmazonS3Client(bucketRegion);
            return new TransferUtility(s3Client);
        }

        private static async Task<Stream> DownloadImage(string imageUrl)
        {
            var response = await client.GetAsync(imageUrl);

            var message = await response.Content.ReadAsStringAsync();

            return message.Contains("Could not access file") 
                ? Stream.Null
                : await response.Content.ReadAsStreamAsync();
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
