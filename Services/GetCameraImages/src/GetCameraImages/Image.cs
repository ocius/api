﻿using Amazon;
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
            var imageUrl = $"https://usvna.ocius.com.au/usvna/oc_server?getliveimage&camera={drone}_{camera}&nowebp&tzoffset=-660";

            Console.WriteLine(imageUrl);

            var response = await client.GetAsync(imageUrl);

            return await IsFailedDownload(response)
                ? CreateEmptyImage(imageUrl)
                : await CreateImage(imageUrl, response);
        }

        private static async Task<bool> IsFailedDownload(HttpResponseMessage response)
        {
            Console.WriteLine(response.StatusCode.ToString());
            if (response.StatusCode != System.Net.HttpStatusCode.OK) return true;
            var message = await response.Content.ReadAsStringAsync();
            Console.WriteLine(message);
            return message.Contains("Could not access file");

        }
        private static DroneImage CreateEmptyImage(string imageUrl)
        {
            return new DroneImage { Data = Stream.Null, Url = imageUrl };
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

            Console.WriteLine("Attempting image upload:");

            using (image as FileStream)
            {
                try
                {
                    await FileTransferUtility.UploadAsync(image, bucketName, path);
                }
                catch (System.Exception e)
                {
                    Console.WriteLine("Failed image upload:");
                    Console.WriteLine($"Drone: {drone}, Camera: {camera}");
                    Console.WriteLine($"path: {path}");
                    Console.WriteLine(e.StackTrace);
                }
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
