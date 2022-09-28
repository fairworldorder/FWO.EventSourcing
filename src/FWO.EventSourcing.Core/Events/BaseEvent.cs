using Newtonsoft.Json;

namespace FWO.EventSourcing.Core.Events
{
    public class BaseEvent
    {
        public BaseEvent()
        {
        }

        //protected BaseEvent(string type)
        //{
        //    Type = type;
        //}

        [JsonProperty("id")]
        public string Id { get; set; }

        //[JsonProperty("version")]
        //public int Version { get; set; }

        /// <summary>
        /// Discriminator property; Used for polymorphic data binding,
        /// when deserializing event objects.
        /// </summary>
        //[JsonProperty("type")]
        //public string Type { get; set; }
    }
}