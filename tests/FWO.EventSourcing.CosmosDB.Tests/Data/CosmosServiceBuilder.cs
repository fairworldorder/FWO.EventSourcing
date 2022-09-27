using FWO.EventSourcing.CosmosDB.Infrastructure;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Moq;

namespace FWO.EventSourcing.CosmosDB.Tests.Data
{
    public class CosmosServiceBuilder : IDisposable
    {
        protected CosmosClient _cosmosClient;
        protected Mock<ILogger<CosmosService>> _mockLogger;
        private string _databaseName;
        private string _containerName;
        private string _partitionKeyPath;


        public CosmosServiceBuilder()
        {
            // default test is run against cosmos emulator
            var connectionString = "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
            _cosmosClient = new CosmosClient(connectionString);
            _mockLogger = new Mock<ILogger<CosmosService>>();
            _databaseName = "unitTests";
            _containerName = "testContainer";
            _partitionKeyPath = "/partitionKey";
        }

        public CosmosServiceBuilder SetDatabaseName(string databaseName)
        {
            _databaseName = databaseName;
            return this;
        }

        public CosmosServiceBuilder SetContainerName(string containerName)
        {
            _containerName = containerName;
            return this;
        }

        public CosmosServiceBuilder SetPartitionKeyPath(string partitionKeyPath)
        {
            _partitionKeyPath = partitionKeyPath;
            return this;
        }

        public CosmosService Build()
        {
            var cosmosService = new CosmosService(client: _cosmosClient, logger: _mockLogger.Object);

            _cosmosClient.CreateDatabaseIfNotExistsAsync(_databaseName).Wait();
            var database = _cosmosClient.GetDatabase(_databaseName);
            database.CreateContainerIfNotExistsAsync(new ContainerProperties
            {
                Id = _containerName,
                PartitionKeyPath = _partitionKeyPath
            }).Wait();

            cosmosService.SetDatabase(_databaseName).SetContainer(_containerName);
            return cosmosService;
        }

        public void Dispose()
        {
            var database = _cosmosClient.GetDatabase(_databaseName);
            database.DeleteAsync().Wait();
        }
    }
}