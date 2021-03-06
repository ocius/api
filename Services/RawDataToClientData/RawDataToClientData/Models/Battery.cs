using System.Collections.Generic;

namespace RawDataToClientData.Models
{
    public class Batteries
    {
        public List<Battery> Tqb { get; set; }

        public Batteries ()
        {
            this.Tqb = new List<Battery>();
        }
    }

    public class Battery
    {
        public string Id { get; set; }
        public string Vol { get; set; }
        public string Cur { get; set; }
        public string Pwr { get; set; }
        public string Pcnt { get; set; }
        public string Time { get; set; }
        public string Age { get; set; }
    }
}
