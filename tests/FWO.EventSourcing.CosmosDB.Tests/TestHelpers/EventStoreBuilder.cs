using FWO.EventSourcing.Core.Infrastructure;
using FWO.EventSourcing.CosmosDB.Tests.TestEvents;

namespace FWO.EventSourcing.CosmosDB.Tests.TestHelpers
{
    public class EventStoreBuilder : IDisposable
    {
        private CosmosEventStoreRepositoryBuilder _repositoryBuilder;
        private TestEventResolver _eventResolver;

        public EventStoreBuilder()
        {
            _repositoryBuilder = new();
            _eventResolver = new();
        }

        public EventStore Build()
        {
            var eventStoreRepository = _repositoryBuilder.Build();
            return new EventStore(eventStoreRepository, _eventResolver);
        }

        public void Dispose()
        {
            _repositoryBuilder.Dispose();
        }
    }
}