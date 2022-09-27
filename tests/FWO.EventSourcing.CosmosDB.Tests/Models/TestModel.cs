using Newtonsoft.Json;

namespace FWO.EventSourcing.CosmosDB.Tests.Models
{
    public class TestModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("partitionKey")]
        public string PartitionKey { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}