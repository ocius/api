using Newtonsoft.Json;

namespace GetLocations
{
    public class Timespan
    {
        [JsonProperty("timespan")]
        public string Value { get; set; }
    }
}
