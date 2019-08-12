namespace GetDrones.Models
{
    public class Props
    {
        public string Wp_dist { get; set; }
        public string Next_wp { get; set; }
        public string Water_depth { get; set; }
        public string Water_temp { get; set; }
        public string Water_speed { get; set; }
        public string Wind_Speed { get; set; }
        public string Wind_direction { get; set; }
        public string Boat_speed { get; set; }
        public string Throttle { get; set; }
        public string Num_Sats { get; set; }
        public string Hdop { get; set; }
        public string Heading { get; set; }
        public Location Location { get; set; }
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
