using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ociusApi.Models;

namespace ociusApi
{
    public class Query
    {
        public static bool IsValidResponse(QueryResponse queryResponse)
        {
            return queryResponse != null && queryResponse.Items != null && queryResponse.Items.Any();
        }

        public static QueryRequest CreateSupportedDronesRequest()
        {
            return new QueryRequest{
                TableName = "APIConfiguration",
                KeyConditionExpression = "Setting = :partitionKeyVal",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> { { ":partitionKeyVal", new AttributeValue { S =  "Drones" } } },
                Limit = 1
            };
        }

        public static List<string> parseSupportedDroneResponse(QueryResponse supportedDronesResponse)
        {
            // assumes every drone has a name, this is a valid assumpuption since the name is the partition key
            // If the table is changed, this may not be a valid assumption
            if (!IsValidResponse(supportedDronesResponse)){
                Console.WriteLine("Invalid supported drones response");
                return new List<string>();
            }
            List<string> droneNames = supportedDronesResponse.Items[0]["Value"]?.S.Split(",").ToList();
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

        public static QueryRequest CreateLatestDronesRequest(string date, string droneName)
        {
            var partitionKeyValue = droneName + date;
            return new QueryRequest
            {
                TableName = "DroneDataSensors",
                KeyConditionExpression = "#partitionKeyName = :partitionKeyValue",
                ExpressionAttributeNames = new Dictionary<string, string> { { "#partitionKeyName", "DroneName+Date" } },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    { ":partitionKeyValue", new AttributeValue { S = partitionKeyValue } },
                    { ":false", new AttributeValue { BOOL = false } }
                },
                FilterExpression = "IsSensitive = :false",
                ScanIndexForward = false,
                Limit = 1
            };
        }

        public static DroneSensor ParseLatestDroneRequest(QueryResponse queryResponse)
        {
            if (!IsValidResponse(queryResponse)){ // Double validity check Database.cs:35
                Console.WriteLine("Invalid latest drone response");
                return new DroneSensor();
            } 
            return DroneSensor.CreateDrone(queryResponse.Items[0]);
        }

        public static QueryRequest CreateDroneByTimeRequestDeprecated(string date, long timestamp, string resource)
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

        public static List<DroneLocation> ParseDroneByTimeRequest(QueryResponse queryResponse)
        {
            if (!IsValidResponse(queryResponse)){
                Console.WriteLine("Invalid drone time span response");
                return new List<DroneLocation>();
            } 
            return queryResponse.Items.Select(loc => DroneLocation.CreateDrone(loc)).ToList();
        }

        public static QueryRequest CreateDroneByTimeRequest(string date, string droneName, long timestamp)
        {
            var partitionKeyValue = droneName + date;
            return new QueryRequest
            {
                TableName = "DroneDataLocations",
                KeyConditionExpression = "#partitionKeyName = :partitionKeyValue and #timespan > :timespan ",
                ExpressionAttributeNames = new Dictionary<string, string> { { "#timespan", "Timestamp" }, { "#partitionKeyName", "DroneName+Date" } },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    { ":partitionKeyValue", new AttributeValue { S = partitionKeyValue } },
                    { ":timespan", new AttributeValue { N = timestamp.ToString() } },
                    { ":false", new AttributeValue { BOOL = false } }
                },
                FilterExpression = "IsSensitive = :false",
                ScanIndexForward = false
            };
        }
    }
}
