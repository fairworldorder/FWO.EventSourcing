using FWO.EventSourcing.CosmosDB.Infrastructure;

namespace FWO.EventSourcing.CosmosDB.Tests.TestHelpers
{
    public class CosmosEventStoreRepositoryBuilder : IDisposable
    {
        private CosmosServiceBuilder _cosmosServiceBuilder;

        public CosmosEventStoreRepositoryBuilder()
        {
            _cosmosServiceBuilder = new();
        }

        public CosmosEventStoreRepository Build()
        {
            var cosmosService = _cosmosServiceBuilder
                .SetPartitionKeyPath("/aggregateId")
                .Build();
            return new CosmosEventStoreRepository(cosmosService);
        }

        public void Dispose()
        {
            _cosmosServiceBuilder.Dispose();
        }
    }
}