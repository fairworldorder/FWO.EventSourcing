using FWO.EventSourcing.Core.Events;
using FWO.EventSourcing.Core.Infrastructure;

namespace FWO.EventSourcing.CosmosDB.Infrastructure
{
    public class CosmosEventStoreRepository : IEventStoreRepository
    {
        private readonly ICosmosService _cosmosService;

        public CosmosEventStoreRepository(ICosmosService cosmosService)
        {
            _cosmosService = cosmosService;
        }

        public async Task<List<EventModel>> LoadByAggregateIdAsync(string aggregateId)
        {
            var documents = await _cosmosService.QueryItemsAsync<EventModel>($"select * from c where c.aggregateId = '{aggregateId}'");
            return documents;
        }

        public async Task SaveAsync(EventModel @event)
        {
            await _cosmosService.InsertDocumentAsync(@event.AggregateId, @event);
        }
    }
}