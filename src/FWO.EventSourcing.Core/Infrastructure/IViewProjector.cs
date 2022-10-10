using FWO.EventSourcing.Core.Domain;

namespace FWO.EventSourcing.Core.Infrastructure
{
    public interface IViewProjector
    {
        T ProjectView<T>(AggregateRoot aggregate) where T : new();
    }
}