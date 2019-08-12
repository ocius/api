using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GetDrones.Models
{
    public class Timespan
    {
        [JsonProperty("timespan")]
        public string Value { get; set; }
    }
}
