using FWO.EventSourcing.Core.Domain;
using FWO.EventSourcing.Core.Handlers;
using FWO.EventSourcing.Core.Infrastructure;
using FWO.EventSourcing.CosmosDB.Tests.TestAggregates;

namespace FWO.EventSourcing.CosmosDB.Tests.TestHandlers
{
    public class UserEventSourcingHandler : IEventSourcingHandler<UserAggregate>
    {
        private readonly IEventStore _eventStore;

        public UserEventSourcingHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<UserAggregate> LoadAggregateByIdAsync(string aggregateId)
        {
            var aggregate = new UserAggregate();
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
                await _eventStore.SaveEventAsync(aggregate.Id, nameof(UserAggregate), uncommitedEvents, aggregate.Version);
                aggregate.CommitChanges();
            }
        }
    }
}