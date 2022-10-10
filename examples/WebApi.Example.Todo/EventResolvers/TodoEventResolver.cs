using FWO.EventSourcing.Core.Events;
using FWO.EventSourcing.Core.Infrastructure;
using Newtonsoft.Json.Linq;
using WebApi.Example.Todo.Events;

namespace WebApi.Example.Todo.EventResolvers
{
    public class TodoEventResolver : IEventResolver
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
                    case nameof(TodoCreated):
                        @event = ((JObject)eventModel.EventData).ToObject<TodoCreated>();
                        break;

                    case nameof(TodoUpdated):
                        @event = ((JObject)eventModel.EventData).ToObject<TodoUpdated>();
                        break;

                    case nameof(TodoDeleted):
                        @event = ((JObject)eventModel.EventData).ToObject<TodoDeleted>();
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
