using FWO.EventSourcing.Core.Events;

namespace FWO.EventSourcing.Core.Infrastructure
{
    /// <summary>
    /// Implements the logic for loading events from a repository.
    /// </summary>
    public interface IEventStoreRepository
    {
        Task<List<EventModel>> LoadByAggregateIdAsync(string aggregateId);

        Task SaveAsync(EventModel @event);
    }
}