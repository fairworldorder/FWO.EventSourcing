using FWO.EventSourcing.CosmosDB.Tests.Data;
using FWO.EventSourcing.CosmosDB.Tests.Models;
using Xunit;

namespace FWO.EventSourcing.CosmosDB.Tests.Infrastructure
{
    public class CosmosServiceTests : IDisposable
    {
        private CosmosServiceBuilder _cosmosServiceBuilder;

        public CosmosServiceTests()
        {
            _cosmosServiceBuilder = new CosmosServiceBuilder();
        }

        public void Dispose()
        {
            _cosmosServiceBuilder.Dispose();
        }

        [Fact]
        public async Task Expect_InsertDocumentAsync_Does_Not_Throw_Exception_When_Valid_Partition_Key_Specified()
        {
            var sut = _cosmosServiceBuilder.Build();
            var partitionKey = Guid.NewGuid().ToString();
            var exception = await Record.ExceptionAsync(async () =>
            {
                await sut.InsertDocumentAsync(partitionKey, new TestModel
                {
                    Id = Guid.NewGuid().ToString(),
                    PartitionKey = partitionKey,
                    Message = "xunit test"
                });
            });

            Assert.Null(exception);
        }

        [Fact]
        public async Task Expect_UpsertDocumentAsync_Does_Not_Throw_Exception_When_Valid_Partition_Key_Specified()
        {
            var sut = _cosmosServiceBuilder.Build();
            var partitionKey = Guid.NewGuid().ToString();
            var documentId = Guid.NewGuid().ToString();

            var exception = await Record.ExceptionAsync(async () =>
            {
                for(var i=0; i<5; i++)
                {
                    await sut.UpsertDocumentAsync(partitionKey, new TestModel
                    {
                        Id = documentId,
                        PartitionKey = partitionKey,
                        Message = $"{i}"
                    });
                }
            });

            Assert.Null(exception);
        }
    }
}