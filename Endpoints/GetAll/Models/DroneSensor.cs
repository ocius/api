﻿using Amazon.DynamoDBv2.Model;
using ociusApi.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ociusApi
{
    public class DroneSensor
    {
        public string Name { get; set; }
        public string Timestamp { get; set; } = "0";
        public string Status { get; set; } = "INVALID";
        public Props Props { get; set; }
        
        public static DroneSensor CreateDrone(Dictionary<string, AttributeValue> attributes)
        {
            var drone = new DroneSensor();
            var coordinates = new Coordinates();
            var location = new Location();
            var props = new Props();

            props.Batteries = new List<string>();
            props.BatteryPercentages = new List<string>();
            props.Cameras = new List<string>();

            foreach (KeyValuePair<string, AttributeValue> kvp in attributes)
            {
                var key = kvp.Key;
                var value = kvp.Value;

                if (key == "Name") drone.Name = value?.S ?? "Not found";
                if (key == "Timestamp") drone.Timestamp = value?.N ?? "Not found";
                if (key == "Status") drone.Status = value?.S ?? "Not found";

                if (key == "Water_depth") props.Water_depth = value?.S ?? "0";
                if (key == "Water_temp") props.Water_temp = value?.S ?? "0";
                if (key == "Wind_speed") props.Wind_speed = value?.S ?? "0";
                if (key == "Wind_direction") props.Wind_direction = value?.S ?? "0";
                if (key == "Boat_speed") props.Boat_speed = value?.S ?? "0";
                if (key == "Heading") props.Heading = value?.S ?? "0";
                if (key == "Batteries") props.Batteries = StringToList(value?.S ?? "");
                if (key == "BatteryPercentages") props.BatteryPercentages = StringToList(value?.S ?? "");
                if (key == "Cameras") props.Cameras = StringToList(value?.S ?? "");

                if (key == "Lat") coordinates.Lat = value?.S ?? "0";
                if (key == "Lon") coordinates.Lon = value?.S ?? "0";
                if (key == "IsSensitive")  Console.WriteLine(value?.BOOL);
            }

            location.Coordinates = coordinates;
            props.Location = location;
            drone.Props = props;

            return drone;
        }
        private static List<string> StringToList(string value)
        {
            return value.Split(",").ToList();
        }
    }
}
