using Newtonsoft.Json;

namespace GetDrones.Models
{
    public class Timespan
    {
        [JsonProperty("timespan")]
        public string Value { get; set; }
    }
}
