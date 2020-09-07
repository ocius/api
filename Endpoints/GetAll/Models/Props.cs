using System.Collections.Generic;

namespace ociusApi
{
    public class Props
    {
        public string Water_depth { get; set; }
        public string Water_temp { get; set; }
        public string Wind_speed { get; set; }
        public string Wind_direction { get; set; }
        public string Boat_speed { get; set; }
        public string Heading { get; set; }
        public List<Battery> Batteries { get; set; }
        public List<string> Cameras { get; set; }
        public Location Location { get; set; }
    }

    public class Battery
    {
        public string Percentage {get; set;}
        public string Voltage {get; set;}
    }
    public class Location
    {
        public Coordinates Coordinates { get; set; }
    }

    public class Coordinates
    {
        public string Lat { get; set; }
        public string Lon { get; set; }
    }
}
