using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ociusApi.Models
{
    public interface IDrone
    {
        string ToJson(QueryResponse queryResponse, bool isLatest);
    }

    public abstract class Drone
    {

        public string Name { get; set; }

        public string ToJson(QueryResponse queryResponse, bool isLatest)
        {
            if (!IsValidResponse(queryResponse)) return "{}";
            
            var drones = queryResponse.Items.Select(item => CreateDrone(item));

            if (isLatest)
            {
                drones = RemoveDuplicates(drones);
            }

            return JsonConvert.SerializeObject(drones);
        }

        public abstract Drone CreateDrone(Dictionary<string, AttributeValue> attributes);

        private static bool IsValidResponse(QueryResponse queryResponse)
        {
            return queryResponse != null && queryResponse.Items != null && queryResponse.Items.Any();
        }

        private static List<Drone> RemoveDuplicates(IEnumerable<Drone> drones)
        {
            var first = drones.First().Name;
            var second = drones.Last().Name;

            return (first == second) ? new List<Drone> { drones.First() } : drones.ToList();
        }
    }
}
