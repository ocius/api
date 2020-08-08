using Newtonsoft.Json.Linq;

namespace RawDataToClientData.Models
{
    public class DroneUtils
    {
        public static string ParseDecimal(JToken token)
        {
            var offset = 10000000;
            var decimalValue = token.ToObject<decimal>();
            return (decimalValue / offset).ToString();
        }
        public static string ParseBattery(Battery battery)
        {
            return battery.Vol.Insert(2, ".");
        }
  }
}
