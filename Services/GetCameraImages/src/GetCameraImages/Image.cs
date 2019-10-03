using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace GetCameraImages
{
    public class DroneImage
    {
        public Stream Data { get; set; }
        public string Url { get; set; }
        public bool HasData => Data.Length != 0;

        private static TransferUtility FileTransferUtility => CreateTransferUtility();
        private static readonly HttpClient client = new HttpClient();

        public static async Task<DroneImage> Download(string drone, string camera)
        {
            var imageUrl = $"https://usvna.ocius.com.au/usvna/oc_server?getliveimage&camera={drone}_{camera}";

            Console.WriteLine("DOWNLOAD URL " + imageUrl);

            var response = await client.GetAsync(imageUrl);

            Console.WriteLine("downloaded image here");

            return await CreateImage(imageUrl, response);
        }

        private static async Task<DroneImage> CreateImage(string imageUrl, HttpResponseMessage response)
        {
            var data = await response.Content.ReadAsStreamAsync();

            return new DroneImage { Data = data, Url = imageUrl };
        }

        public static async Task<string> Upload(Stream image, string drone, string camera, string timestamp)
        {
            var prettyUrl = camera.Replace("%20", "").Replace("_", "");

            var path = $"{drone}/{timestamp}/{prettyUrl}.jpg";

            var bucketName = "ocius-images";

            using (image as FileStream)
            {
                await FileTransferUtility.UploadAsync(image, bucketName, path);
            }

            return path;
        }

        private static TransferUtility CreateTransferUtility()
        {
            var bucketRegion = RegionEndpoint.APSoutheast2;
            var s3Client = new AmazonS3Client(bucketRegion);
            return new TransferUtility(s3Client);
        }
    }
}
