using Newtonsoft.Json.Linq;

namespace RawDataToClientData.Models
{
    public class DroneUtils
    {
        public static string ParseCoordinates(JToken token)
        {
            var offset = 10000000;
            var coordinate = token.ToObject<decimal>();
            return (coordinate / offset).ToString();
        }
    }
}
