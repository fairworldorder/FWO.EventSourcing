using FWO.EventSourcing.Core.Domain;

namespace FWO.EventSourcing.Core.Handlers
{
    public interface IEventSourcingHandler<T> where T : AggregateRoot
    {
        Task<T> LoadAggregateByIdAsync(string aggregateId);

        Task SaveAsync(AggregateRoot aggregate);
    }
}