using System.Data;
using FWO.EventSourcing.Core.Events;
using FWO.EventSourcing.Core.Exceptions;

namespace FWO.EventSourcing.Core.Infrastructure
{
    public class EventStore : IEventStore
    {
        private IEventStoreRepository _repository;
        private IEventResolver _eventResolver;

        public EventStore(IEventStoreRepository repository, IEventResolver eventResolver)
        {
            _repository = repository;
            _eventResolver = eventResolver;
        }

        public async Task<List<BaseEvent>> LoadEventsAsync(string aggregateId)
        {
            var eventStream = await _repository.LoadByAggregateIdAsync(aggregateId);
            if (eventStream == null || !eventStream.Any())
                throw new AggregateNotFoundException($"PostId {aggregateId} not found");

            // Resolve the EventData within each EventModel to a derived BaseEvent type.
            var events = _eventResolver.ResolveEvents(eventStream.OrderBy(x => x.Version).ToList());
            return events;
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
                var eventType = @event.GetType().Name;
                var eventModel = new EventModel
                {
                    Id = $"{aggregateId}_{version}",
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