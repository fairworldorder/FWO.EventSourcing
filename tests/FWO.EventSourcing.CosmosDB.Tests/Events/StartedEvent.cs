using FWO.EventSourcing.Core.Events;
using Newtonsoft.Json;

namespace FWO.EventSourcing.CosmosDB.Tests.Events
{
    public class StartedEvent : BaseEvent
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }
    }
}