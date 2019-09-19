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
        private const string bucketName = "ocius-images";
        private const string keyName = "tomamyTest2.jpg";

        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.APSoutheast2;
        private static readonly IAmazonS3 s3Client = new AmazonS3Client(bucketRegion);
        private readonly HttpClient client = new HttpClient();
        private readonly TransferUtility fileTransferUtility = new TransferUtility(s3Client);

        public async Task<string> FunctionHandler()
        {
            var image = await DownloadImage();
            await UploadImage(image);

            //get url
            //url is baseUrl + folderName + keyName
            //Key name coudl be a timestamp

            //add to a DB
            //save url to DB for fetching

            return $"Image {keyName} uploaded";
        }

        private async Task<Stream> DownloadImage()
        {
            var response = await client.GetAsync("https://tomamy.co/amytom.jpg");
            return await response.Content.ReadAsStreamAsync();
        }

        private async Task UploadImage(Stream image)
        {
            using (image as FileStream)
            {
                await fileTransferUtility.UploadAsync(image, bucketName, keyName);
            }
        }

    }
}
