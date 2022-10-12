using Microsoft.Azure.Cosmos;

namespace FWO.EventSourcing.CosmosDB.Infrastructure
{
    public interface ICosmosService
    {
        ICosmosService SetDatabase(string databaseName);

        ICosmosService SetContainer(string containerName);

        Task<ItemResponse<T>> InsertDocumentAsync<T>(string partitionKey, T value, CancellationToken ct = default);

        Task<ItemResponse<T>> UpsertDocumentAsync<T>(string partitionKey, T value, CancellationToken ct = default);

        Task<List<T>> QueryItemsAsync<T>(string query, CancellationToken ct = default);
    }
}