using FWO.EventSourcing.Core.Events;
using FWO.EventSourcing.CosmosDB.Tests.TestEvents;

namespace FWO.EventSourcing.CosmosDB.Tests.TestHelpers
{
    public class EventListBuilder
    {
        private List<BaseEvent> _events;

        public EventListBuilder()
        {
            _events = new List<BaseEvent>();
        }

        public EventListBuilder SetDefault()
        {
            _events = new List<BaseEvent>
            {
                new StartedEvent
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "John",
                    LastName = "Doe",
                    //Type = nameof(StartedEvent),
                },
                new ModifiedEvent
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "Joe",
                    LastName = "Schmoe",
                    //Type = nameof(ModifiedEvent),
                },
                new DeletedEvent
                {
                    Id = Guid.NewGuid().ToString(),
                    //Type = nameof(DeletedEvent)
                }
            };

            return this;
        }
        public EventListBuilder Clear()
        {
            _events = new List<BaseEvent>();
            return this;
        }

        public EventListBuilder AddStartedEvent(string firstName, string lastName, DateTime dateOfBirth)
        {
            _events.Add(new StartedEvent
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                //Type = nameof(StartedEvent)
            });

            return this;
        }

        public EventListBuilder AddModifiedEvent(string firstName, string lastName)
        {
            _events.Add(new ModifiedEvent
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = firstName,
                LastName = lastName,
                //Type = nameof(ModifiedEvent)
            });

            return this;
        }

        public EventListBuilder AddDeletedEvent()
        {
            _events.Add(new DeletedEvent
            {
                Id = Guid.NewGuid().ToString(),
                //Type = nameof(DeletedEvent)
            });

            return this;
        }

        public List<BaseEvent> Build()
        {
            return _events;
        }
    }
}