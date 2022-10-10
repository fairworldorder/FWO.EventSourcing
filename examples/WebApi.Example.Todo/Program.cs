using FWO.EventSourcing.Core.Handlers;
using FWO.EventSourcing.Core.Infrastructure;
using FWO.EventSourcing.CosmosDB.Infrastructure;
using FWO.EventSourcing.CosmosDB.Options;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WebApi.Example.Todo.Aggregates;
using WebApi.Example.Todo.EventResolvers;
using WebApi.Example.Todo.EventSourcingHandlers;
using WebApi.Example.Todo.MaterialisedViews;

namespace WebApi.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Event sourcing
            builder.Services.Configure<CosmosDbOptions>(builder.Configuration.GetSection(nameof(CosmosDbOptions)));
            builder.Services.AddSingleton<CosmosClient>(sp =>
            {
                var cosmosClientOptions = sp.GetRequiredService<IOptions<CosmosDbOptions>>();
                return new CosmosClient(cosmosClientOptions.Value.ConnectionString);
            });

            builder.Services.AddScoped<ICosmosService>(sp =>
            {
                var cosmosClient = sp.GetRequiredService<CosmosClient>();
                var logger = sp.GetRequiredService<ILogger<CosmosService>>();
                var cosmosService = new CosmosService(cosmosClient, logger);
                cosmosService.SetDatabase("todo")
                             .SetContainer("todo-events");
                return cosmosService;
            });

            builder.Services.AddScoped<IEventStoreRepository, CosmosEventStoreRepository>();
            builder.Services.AddScoped<IEventResolver, TodoEventResolver>();
            builder.Services.AddScoped<IEventStore, EventStore>();

            builder.Services.AddScoped<IEventSourcingHandler<TodoAggregate>, TodoEventSourcingHandler>();

            // View projectors
            builder.Services.AddScoped<IViewProjector, AllTodosViewProjector>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}