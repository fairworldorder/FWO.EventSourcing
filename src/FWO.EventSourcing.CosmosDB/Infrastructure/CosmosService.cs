using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace FWO.EventSourcing.CosmosDB.Infrastructure
{
    public class CosmosService : ICosmosService
    {
        private CosmosClient _client;
        private ILogger<CosmosService> _logger;
        private Database _database;
        private Container _container;

        public CosmosService(CosmosClient client, ILogger<CosmosService> logger)
        {
            _client = client;
            _logger = logger;
            _database = null;
            _container = null;
        }

        public ICosmosService SetDatabase(string databaseName)
        {
            _database = _client.GetDatabase(databaseName);
            return this;
        }

        public ICosmosService SetContainer(string containerName)
        {
            _container = _database.GetContainer(containerName);
            return this;
        }

        public async Task<ItemResponse<T>> InsertDocumentAsync<T>(string partitionKey,
                                                                  T value,
                                                                  CancellationToken ct = default)
        {
            ThrowIfNotReady();
            var pkey = new PartitionKey(partitionKey);
            var response = await _container.CreateItemAsync<T>(item: value,
                                                               partitionKey: pkey,
                                                               cancellationToken: ct);

            _logger.LogDebug("Insert cost {requestCharge} RUs.", response.RequestCharge);
            return response;
        }

        public async Task<ItemResponse<T>> UpsertDocumentAsync<T>(string partitionKey,
                                                                  T value,
                                                                  CancellationToken ct = default)
        {
            ThrowIfNotReady();
            var pkey = new PartitionKey(partitionKey);
            var response = await _container.UpsertItemAsync<T>(item: value,
                                                               partitionKey: pkey,
                                                               cancellationToken: ct);

            _logger.LogDebug("Upsert cost {requestCharge} RUs.", response.RequestCharge);
            return response;
        }

        public async Task<List<T>> QueryItemsAsync<T>(string query, CancellationToken ct = default)
        {
            ThrowIfNotReady();
            var queryDefinition = new QueryDefinition(query);
            using FeedIterator<T> queryResultSetIterator = _container.GetItemQueryIterator<T>(queryDefinition);

            var results = new List<T>();
            double requestCharge = 0;

            while (queryResultSetIterator.HasMoreResults)
            {
                var currentResultSet = await queryResultSetIterator.ReadNextAsync(ct);
                foreach (T item in currentResultSet)
                {
                    results.Add(item);
                }
                requestCharge += currentResultSet.RequestCharge;
            }

            _logger.LogDebug("Query cost {requestCharge} RUs", requestCharge);
            return results;
        }

        private void ThrowIfNotReady()
        {
            if (_container == null || _database == null)
                throw new Exception("Database or container has not been set.");
        }
    }
}