using Newtonsoft.Json;

namespace FWO.EventSourcing.Core.Events
{
    /// <summary>
    /// Represents a document in the event store collection;
    /// Each document represents an event that is versioned that can alter the state of the aggregate.
    /// </summary>
    public class EventModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("aggregateId")]
        public string AggregateId { get; set; }

        [JsonProperty("aggregateType")]
        public string AggregateType { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("eventType")]
        public string EventType { get; set; }

        [JsonProperty("eventData")]
        public BaseEvent EventData { get; set; }
    }
}