using FWO.EventSourcing.Core.Domain;
using FWO.EventSourcing.Core.Handlers;
using FWO.EventSourcing.Core.Infrastructure;
using WebApi.Example.Todo.Aggregates;

namespace WebApi.Example.Todo.EventSourcingHandlers
{
    public class TodoEventSourcingHandler : IEventSourcingHandler<TodoAggregate>
    {
        private readonly IEventStore _eventStore;

        public TodoEventSourcingHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<TodoAggregate> LoadAggregateByIdAsync(string aggregateId)
        {
            var aggregate = new TodoAggregate();
            var events = await _eventStore.LoadEventsAsync(aggregateId);
            if (events != null && events.Any())
                aggregate.LoadEvents(events, aggregateId);

            return aggregate;
        }

        public async Task SaveAsync(AggregateRoot aggregate)
        {
            var uncommitedEvents = aggregate.GetUncommittedChanges();
            if (uncommitedEvents.Any())
            {
                await _eventStore.SaveEventAsync(aggregate.Id, nameof(TodoAggregate), uncommitedEvents, aggregate.Version);
                aggregate.CommitChanges();
            }
        }
    }
}
