using FWO.EventSourcing.Core.Events;
using Newtonsoft.Json;

namespace FWO.EventSourcing.CosmosDB.Tests.TestEvents
{
    public class ModifiedEvent : BaseEvent
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }
    }
}