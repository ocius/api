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
            var drones = new List<DroneSensor>();

            var droneRequestTasks = new List<Task<QueryResponse>>();

            foreach (var droneName in supportedDroneNames)
            {
                var latestDronesRequest = Query.CreateLatestDronesRequest(date, droneName);
                droneRequestTasks.Add(client.QueryAsync(latestDronesRequest));
            }

            var databaseResponses = await Task.WhenAll(droneRequestTasks);

            foreach (var databaseResponse in databaseResponses)
            {
                if (!Query.IsValidResponse(databaseResponse))
                {
                    continue;
                }
                var drone = Query.ParseLatestDroneRequest(databaseResponse);
                drones.Add(drone);
            }
            return drones;
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
            var timeSpan = GetTimespan(timePeriod);

            var timespanRequestTasks = new List<Task<QueryResponse>>();
            foreach (var droneName in supportedDroneNames)
            {
                var dronesByTimespanRequest = Query.CreateDroneByTimeRequest(date, droneName, timeSpan);
                timespanRequestTasks.Add(client.QueryAsync(dronesByTimespanRequest));
            }

            var databaseResponses = await Task.WhenAll(timespanRequestTasks);

            foreach (var databaseResponse in databaseResponses)
            {
                if (!Query.IsValidResponse(databaseResponse))
                {
                    //Console.WriteLine($"No timeline found for {droneName} from {date} to {timeSpan}");
                    continue;
                }
                var droneTimespan = Query.ParseDroneByTimeRequest(databaseResponse);
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
    }
}
