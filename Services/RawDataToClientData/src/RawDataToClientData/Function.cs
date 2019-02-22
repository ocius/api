using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace RawDataToClientData
{
    public class Function
    {
        public void FunctionHandler(ILambdaContext context)
        {
            var rawData = ReadRawData();
            var clientData = TransformData(rawData);
            SaveToDatabase(clientData);
        }

        public static string ReadRawData()
        {
            var rawData = @"{
                ""foo"": ""bar"", 
                ""x"": ""y"", 
                ""1"": ""0""
            }";

            return rawData;
        }
        public static string TransformData(string rawData)
        {
            var clientData = @"{
                ""drone"": ""bruce"", 
                ""lat"": ""1"", 
                ""long"": ""1""
            }";

            return clientData;
        }

        public static void SaveToDatabase(string clientData){

            Console.WriteLine("Save to Database");
        }
    }
}
