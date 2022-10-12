using Newtonsoft.Json;

namespace FWO.EventSourcing.Core.Events
{
    public class BaseEvent
    {
        public BaseEvent()
        {
        }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}