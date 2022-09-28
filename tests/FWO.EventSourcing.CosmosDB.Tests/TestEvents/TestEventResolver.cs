using FWO.EventSourcing.Core.Events;
using FWO.EventSourcing.Core.Infrastructure;
using Newtonsoft.Json.Linq;

namespace FWO.EventSourcing.CosmosDB.Tests.TestEvents
{
    public class TestEventResolver : IEventResolver
    {
        public List<BaseEvent> ResolveEvents(List<EventModel> eventModels)
        {
            var events = new List<BaseEvent>();

            foreach (var eventModel in eventModels)
            {
                var eventTypeDiscriminator = eventModel.EventType;
                BaseEvent @event;

                switch (eventTypeDiscriminator)
                {
                    case nameof(StartedEvent):
                        @event = ((JObject)eventModel.EventData).ToObject<StartedEvent>();
                        break;

                    case nameof(ModifiedEvent):
                        @event = ((JObject)eventModel.EventData).ToObject<ModifiedEvent>();
                        break;

                    case nameof(DeletedEvent):
                        @event = ((JObject)eventModel.EventData).ToObject<DeletedEvent>();
                        break;

                    default:
                        throw new NotImplementedException($"Unrecognized event type: {eventTypeDiscriminator}");
                }

                events.Add(@event);
            }

            return events;
        }
    }
}