using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ociusApi.Models;

namespace ociusApi
{
    public class Query
    {
        private static bool IsValidResponse(QueryResponse queryResponse)
        {
            return queryResponse != null && queryResponse.Items != null && queryResponse.Items.Any();
        }

        private static bool IsValidResponse(ScanResponse scanResponse)
        {
            return scanResponse != null && scanResponse.Items != null && scanResponse.Items.Any();
        }

        public static ScanRequest CreateSupportedDronesRequest()
        {
            return new ScanRequest{
                TableName = "DroneStatus"
            };
        }

        public static List<string> parseSupportedDroneResponse(ScanResponse supportedDronesResponse)
        {
            // assumes every drone has a name, this is a valid assumpuption since the name is the partition key
            // If the table is changed, this may not be a valid assumption
            if (!IsValidResponse(supportedDronesResponse)) return new List<string>();
            List<string> droneNames = supportedDronesResponse.Items.Select(item => item["Name"]?.S).ToList();
            return droneNames;
        }

        public static QueryRequest CreateLatestDroneRequestDeprecated(string resource)
        {
            var Date = DateTime.UtcNow.ToString("yyyyMMdd");

            return new QueryRequest
            {
                TableName = resource,
                KeyConditionExpression = "#date = :date",
                ExpressionAttributeNames = new Dictionary<string, string> { { "#date", "Date" } },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> { { ":date", new AttributeValue { N = Date } } },
                ScanIndexForward = false,
                Limit = 3 // This should come from the database
            };
        }

        public static QueryRequest CreateLatestDronesRequest(string resource, string droneName)
        {
            var date = DateTime.UtcNow.ToString("yyyyMMdd");
            var partitionKey = droneName + date;
            return new QueryRequest
            {
                TableName = resource,
                KeyConditionExpression = "DroneName+Date = :partitionKey",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    { ":partitionKey", new AttributeValue { N = partitionKey } },
                    { ":true", new AttributeValue { BOOL = true } }
                },
                FilterExpression = "IsSensitive =:true",
                ScanIndexForward = false,
                Limit = 1
            };
        }

        public static DroneSensor ParseLatestDroneRequest(QueryResponse queryResponse)
        {
            if (!IsValidResponse(queryResponse)) return new DroneSensor();
            return DroneSensor.CreateDrone(queryResponse.Items[0]);
        }

        public static QueryRequest CreateDroneByTimeRequest(string date, long timestamp, string resource)
        {
            return new QueryRequest
            {
                TableName = resource,
                KeyConditionExpression = "#date = :date and #timespan > :timespan ",
                ExpressionAttributeNames = new Dictionary<string, string> { { "#timespan", "Timestamp" }, { "#date", "Date" } },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    { ":timespan", new AttributeValue { N = timestamp.ToString() } },
                    { ":date", new AttributeValue { N = date } },
                }
            };
        }
    }
}
