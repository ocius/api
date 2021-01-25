using Amazon.DynamoDBv2.Model;
using ociusApi.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ociusApi
{
    public class DroneSensor
    {
        public string Name { get; set; } = "Unknown Name";
        public string Timestamp { get; set; } = "0";
        public string Status { get; set; } = "INVALID";
        public Props Props { get; set; } = new Props();
        
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

                switch (key)
                {
                    case "Name": drone.Name = value?.S ?? "Not found"; break;
                    case "Timestamp": drone.Timestamp = value?.N ?? "Not found"; break;
                    case "Status": drone.Status = value?.S ?? "Not found"; break;
                    case "Water_depth": props.Water_depth = value?.S ?? "0"; break;
                    case "Water_temp": props.Water_temp = value?.S ?? "0"; break;
                    case "Wind_speed": props.Wind_speed = value?.S ?? "0"; break;
                    case "Wind_direction": props.Wind_direction = value?.S ?? "0"; break;
                    case "Current_speed": props.Current_speed = value?.S ?? "0"; break;
                    case "Current_direction": props.Current_direction = value?.S ?? "0"; break;
                    case "Boat_speed": props.Boat_speed = value?.S ?? "0"; break;
                    case "Heading": props.Heading = value?.S ?? "0"; break;
                    case "Batteries": props.Batteries = StringToList(value?.S ?? ""); break;
                    case "BatteryPercentages": props.BatteryPercentages = StringToList(value?.S ?? ""); break;
                    case "Cameras": props.Cameras = StringToList(value?.S ?? ""); break;
                    case "Lat": coordinates.Lat = value?.S ?? "0"; break;
                    case "Lon": coordinates.Lon = value?.S ?? "0"; break;
                }
            }

            location.Coordinates = coordinates;
            props.Location = location;
            drone.Props = props;

            return drone;
        }

        public static bool IsValidDrone(DroneSensor drone) {
            return (drone?.Status ?? "INVALID") != "INVALID";
        }

        private static List<string> StringToList(string value)
        {
            return value.Split(",").ToList();
        }
    }
}
