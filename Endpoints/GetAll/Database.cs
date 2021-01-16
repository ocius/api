using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ociusApi.Models;

namespace ociusApi
{
    public static class Database
    {
        private static readonly AmazonDynamoDBClient client = new AmazonDynamoDBClient();

        public async static Task<List<string>> GetSupportedDrones()
        {
            var supportedDronesRequest = Query.CreateSupportedDronesRequest();
            var response = await client.QueryAsync(supportedDronesRequest);
            return Query.ParseSupportedDroneResponse(response);
        }

        public async static Task<QueryResponse> GetLatestDeprecated(string resource)
        {
            var latestDronesRequest = Query.CreateLatestDroneRequestDeprecated(resource);
            return await client.QueryAsync(latestDronesRequest); ;
        }

        public async static Task<List<DroneSensor>> GetLatest(string date, List<string> supportedDroneNames)
        {
            var droneRequestTasks = new List<Task<DroneSensor>>();

            foreach (var droneName in supportedDroneNames)
            {
                droneRequestTasks.Add(QueryClientForDroneAsync(date, droneName));
            }

            var drones = await Task.WhenAll(droneRequestTasks);

            return new List<DroneSensor>(drones.Where(drone => drone.Status != "INVALID"));
        }

        public async static Task<QueryResponse> GetByTimespanDeprecated(string date, string timePeriod, string resource)
        {
            if (!IsValidTimePeriod(timePeriod)) return new QueryResponse();

            var timeSpan = GetTimespan(timePeriod);

            var dronesByTimespanRequest = Query.CreateDroneByTimeRequestDeprecated(date, timeSpan, resource);

            return await client.QueryAsync(dronesByTimespanRequest);
        }

        public async static Task<List<DroneLocation>> GetByTimespan(string date, List<string> supportedDroneNames, string timePeriod)
        {
            var droneTimespans = new List<DroneLocation>();

            if (!IsValidTimePeriod(timePeriod)) return droneTimespans;
            var timespan = GetTimespan(timePeriod);

            var timespanRequestTasks = new List<Task<List<DroneLocation>>>();

            foreach (var droneName in supportedDroneNames)
            {
                timespanRequestTasks.Add(QueryClientForTimespanAsync(date, droneName, timespan));
            }

            var databaseResponses = await Task.WhenAll(timespanRequestTasks);

            foreach (List<DroneLocation> droneTimespan in databaseResponses)
            {
                droneTimespans.AddRange(droneTimespan);
            }

            return droneTimespans;
        }

        public static bool IsValidTimePeriod(string timespan)
        {
            var validTimespans = new List<string> { "minute", "hour", "day" };

            return (validTimespans.Contains(timespan));
        }

        public static long GetTimespan(string timeSpan)
        {
            var oneMinuteMilliseconds = 60000;
            var oneHourMilliseconds = 3600000;
            var oneDayMilliseconds = 86400000;

            if (timeSpan == "day")
                return GetByTime(oneDayMilliseconds);

            if (timeSpan == "hour")
                return GetByTime(oneHourMilliseconds);

            return GetByTime(oneMinuteMilliseconds);
        }

        private static long GetByTime(int milliseconds)
        {
            var currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            return currentTimestamp - milliseconds;
        }
        private async static Task<DroneSensor> QueryClientForDroneAsync(string date, string droneName)
        {
            var latestDronesRequest = Query.CreateLatestDronesRequest(date, droneName);
            QueryResponse databaseResponse = await client.QueryAsync(latestDronesRequest);

            if (!Query.IsValidResponse(databaseResponse))
            {
                Console.WriteLine($"No entries found for {droneName}");
                return new DroneSensor();
            }

            return Query.ParseLatestDroneRequest(databaseResponse);
        }

        private async static Task<List<DroneLocation>> QueryClientForTimespanAsync(string date, string droneName, long timespan)
        {
            var dronesByTimespanRequest = Query.CreateDroneByTimeRequest(date, droneName, timespan);
            QueryResponse databaseResponse = await client.QueryAsync(dronesByTimespanRequest);

            if (!Query.IsValidResponse(databaseResponse))
            {
                Console.WriteLine($"No timeline found for {droneName} from {date} to {timespan}");
                return new List<DroneLocation>();
            }

            return Query.ParseDroneByTimeRequest(databaseResponse);
        }
    }
}
