using System.Data;
using FWO.EventSourcing.Core.Events;
using FWO.EventSourcing.Core.Exceptions;

namespace FWO.EventSourcing.Core.Infrastructure
{
    public class EventStore : IEventStore
    {
        private IEventStoreRepository _repository;

        public EventStore(IEventStoreRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<BaseEvent>> LoadEventsAsync(string aggregateId)
        {
            var eventStream = await _repository.LoadByAggregateIdAsync(aggregateId);
            if (eventStream == null || !eventStream.Any())
                throw new AggregateNotFoundException($"PostId {aggregateId} not found");

            return eventStream.OrderBy(x => x.Version).Select(x => x.EventData).ToList();
        }

        public async Task SaveEventAsync(string aggregateId, string aggregateType, IEnumerable<BaseEvent> events, int expectedVersion)
        {
            var eventStream = await _repository.LoadByAggregateIdAsync(aggregateId);

            // Optimistic concurrency check
            if (expectedVersion != -1 && eventStream[^1].Version != expectedVersion)
                throw new ConcurrencyException($"{eventStream[^1].Version} does not match expected version: {expectedVersion}.");

            var version = expectedVersion;

            foreach (var @event in events)
            {
                version++;
                @event.Version = version;
                var eventType = @event.GetType().Name;
                var eventModel = new EventModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Timestamp = DateTime.UtcNow,
                    AggregateId = aggregateId,
                    AggregateType = aggregateType,
                    EventType = eventType,
                    EventData = @event,
                    Version = version
                };

                await _repository.SaveAsync(eventModel);
            }
        }
    }
}