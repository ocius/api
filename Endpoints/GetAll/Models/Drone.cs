using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ociusApi.Models
{
    public interface IDrone
    {
        string ToJson(QueryResponse queryResponse);
    }

    public abstract class Drone
    {
        public string ToJson(QueryResponse queryResponse)
        {
            if (!IsValidResponse(queryResponse)) return "{}";

            var drones = queryResponse.Items.Select(item => CreateDrone(item));

            return JsonConvert.SerializeObject(drones);
        }

        public abstract Drone CreateDrone(Dictionary<string, AttributeValue> attributes);

        private static bool IsValidResponse(QueryResponse queryResponse)
        {
            return queryResponse != null && queryResponse.Items != null && queryResponse.Items.Any();
        }
    }
}
