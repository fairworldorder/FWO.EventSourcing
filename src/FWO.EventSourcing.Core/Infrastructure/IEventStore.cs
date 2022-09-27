using FWO.EventSourcing.Core.Events;

namespace FWO.EventSourcing.Core.Infrastructure
{
    /// <summary>
    /// Implements optimistic concurrency checks and logic for loading events from a
    /// <see cref="IEventStoreRepository"/>.
    /// </summary>
    public interface IEventStore
    {
        Task<List<BaseEvent>> LoadEventsAsync(string aggregateId);

        Task SaveEventAsync(string aggregateId, string aggregateType, IEnumerable<BaseEvent> events, int expectedVersion);
    }
}