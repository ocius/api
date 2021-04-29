using System.Collections.Generic;

namespace ociusApi
{
    public class Props
    {
        public string Air_temp { get; set; } = "0";
        public string Water_depth { get; set; } = "0";
        public string Water_temp { get; set; } = "0";
        public string Wind_speed { get; set; } = "0";
        public string Wind_direction { get; set; } = "0";
        public string Current_speed { get; set; } = "0";
        public string Current_direction { get; set; } = "0";
        public string Boat_speed { get; set; } = "0";
        public string Heading { get; set; } = "0";
        public List<string> Batteries { get; set; } = new List<string>();
        public List<string> BatteryPercentages { get; set; } = new List<string>();
        public List<string> Cameras { get; set; } = new List<string>();
        public Location Location { get; set; } 
    }

    public class Location
    {
        public Coordinates Coordinates { get; set; }
    }

    public class Coordinates
    {
        public string Lat { get; set; } = "0";
        public string Lon { get; set; } = "0";
    }
}
