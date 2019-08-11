using Newtonsoft.Json;

namespace ociusApi
{
    public class Timespan
    {
        [JsonProperty("timespan")]
        public string Value { get; set; }
    }
}
