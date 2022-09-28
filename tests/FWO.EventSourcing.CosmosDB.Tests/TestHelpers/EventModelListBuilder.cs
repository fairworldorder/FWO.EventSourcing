using FWO.EventSourcing.Core.Events;
using FWO.EventSourcing.CosmosDB.Tests.TestEvents;

namespace FWO.EventSourcing.CosmosDB.Tests.TestHelpers
{
    public class EventModelListBuilder
    {
        private List<EventModel> _eventModels;

        public EventModelListBuilder()
        {
            _eventModels = new();
        }

        public EventModelListBuilder SetDefault()
        {
            var aggregateId = default(Guid).ToString();

            _eventModels = new List<EventModel>
            {
                new EventModel
                {
                    Id = $"{aggregateId}_0",
                    AggregateId = aggregateId,
                    AggregateType = "TestAggregate",
                    EventType = nameof(StartedEvent),
                    EventData = new StartedEvent
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        Id = Guid.NewGuid().ToString(),
                        //Type = nameof(StartedEvent),
                        //Version = 0,
                    },
                    Timestamp = DateTime.UtcNow,
                    Version = 0
                },
                new EventModel
                {
                    Id = $"{aggregateId}_1",
                    AggregateId = aggregateId,
                    AggregateType = "TestAggregate",
                    EventType = nameof(ModifiedEvent),
                    EventData = new ModifiedEvent
                    {
                        FirstName = "Joe",
                        LastName = "Schmoe",
                        Id = Guid.NewGuid().ToString(),
                        //Type = nameof(ModifiedEvent),
                        //Version = 1
                    },
                        Version = 1
                    },
                    new EventModel
                    {
                        Id = $"{aggregateId}_2",
                        AggregateId = aggregateId,
                        AggregateType = "TestAggregate",
                        EventType = nameof(DeletedEvent),
                        EventData = new DeletedEvent
                        {
                            Id = Guid.NewGuid().ToString(),
                            //Type = nameof(DeletedEvent),
                            //Version = 2
                        },
                         Version = 2
                    }
            };
            return this;
        }

        public List<EventModel> Build()
        {
            return _eventModels;
        }
    }
}