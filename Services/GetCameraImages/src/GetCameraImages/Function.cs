using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.APSoutheast2;
        private static readonly IAmazonS3 s3Client = new AmazonS3Client(bucketRegion);
        private readonly HttpClient client = new HttpClient();
        private readonly TransferUtility fileTransferUtility = new TransferUtility(s3Client);

        private const string bucketName = "ocius-images";


        public async Task<List<string>> FunctionHandler()
        {
            var supportedDrones = new List<string> { "bob", "bruce" };

            var baseUrl = "https://usvna.ocius.com.au/usvna/oc_server?getliveimage&camera=";
            var cameras = new List<string> { "mast" };
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var result = new List<string>();

            var Total = new Stopwatch();
            Total.Start();

            foreach (var drone in supportedDrones)
            {
                foreach(var camera in cameras)
                {
                    var Loop = new Stopwatch();
                    Loop.Start();

                    var imageUrl = $"{baseUrl}{drone}%20{camera}";
                    var image = await DownloadImage(imageUrl);
                    var path = $"{drone}/{timestamp}/{camera}.jpg";
                    await UploadImage(image, path);
                    result.Add(path);

                    Loop.Stop();
                    var elapsed = Total.Elapsed;
                    result.Add("Drone time: " + elapsed.TotalSeconds.ToString());
                }
            }

            Total.Stop();
            var stopwatchElapsed = Total.Elapsed;
            result.Add("Total time: " + stopwatchElapsed.TotalSeconds.ToString());

            return result;
        }

        private async Task<Stream> DownloadImage(string url)
        {
            var response = await client.GetAsync(url);
            return await response.Content.ReadAsStreamAsync();
        }

        private async Task UploadImage(Stream image, string path)
        {
            using (image as FileStream)
            {
                await fileTransferUtility.UploadAsync(image, bucketName, path);
            }
        }

    }
}
