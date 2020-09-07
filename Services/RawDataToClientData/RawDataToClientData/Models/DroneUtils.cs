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
        public static string ParseVoltage(Battery battery)
        {
            var voltage = battery.Vol;
            return voltage.Insert(voltage.Length - 1, ".");
        }
        public static string ParseBattery(Battery battery)
        {
            return new Dictionary<string, AttributeValue>(){
                { "Voltage", new AttributeValue {"S" = battery.ParseVoltage(battery)}},
                { "Percentage", new  AttributeValue {"S" = battery.Pcnt}}
            };
        }
    }
}
