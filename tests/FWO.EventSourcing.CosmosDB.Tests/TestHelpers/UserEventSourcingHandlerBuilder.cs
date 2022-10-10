using FWO.EventSourcing.CosmosDB.Tests.TestHandlers;

namespace FWO.EventSourcing.CosmosDB.Tests.TestHelpers
{
    public class UserEventSourcingHandlerBuilder : IDisposable
    {
        private EventStoreBuilder _eventStoreBuilder;

        public UserEventSourcingHandlerBuilder()
        {
            _eventStoreBuilder = new EventStoreBuilder();
        }

        public UserEventSourcingHandler Build()
        {
            var eventStore = _eventStoreBuilder.Build();

            return new UserEventSourcingHandler(eventStore);
        }

        public void Dispose()
        {
            _eventStoreBuilder.Dispose();
        }
    }
}