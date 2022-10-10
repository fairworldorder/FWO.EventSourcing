using FWO.EventSourcing.Core.Exceptions;
using FWO.EventSourcing.Core.Handlers;
using FWO.EventSourcing.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using WebApi.Example.Todo.Aggregates;
using WebApi.Example.Todo.MaterialisedViews;
using WebApi.Example.Todo.Models;
using WebApi.Example.Todo.Requests;

namespace WebApi.Example.Controllers
{
    [ApiController]
    [Route("todos")]
    public class TodoController : ControllerBase
    {
        private readonly ILogger<TodoController> _logger;
        private readonly IEventSourcingHandler<TodoAggregate> _eventSourcingHandler;
        private readonly IViewProjector _viewProjector;

        public TodoController(ILogger<TodoController> logger,
                              IEventSourcingHandler<TodoAggregate> eventSourcingHandler,
                              IViewProjector viewProjector)
        {
            _logger = logger;
            _eventSourcingHandler = eventSourcingHandler;
            _viewProjector = viewProjector;
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> CreateTodo([FromRoute] string userId, [FromBody] CreateTodoRequest request)
        {
            TodoAggregate aggregate;
            try
            {
                aggregate = await _eventSourcingHandler.LoadAggregateByIdAsync(userId);
                aggregate.CreateTodo(request.Data, DateTime.UtcNow);
            }
            catch (AggregateNotFoundException)
            {
                aggregate = new TodoAggregate(userId, request.Data, DateTime.UtcNow);
            }

            await _eventSourcingHandler.SaveAsync(aggregate);
            return new OkResult();
        }

        [HttpGet("{userId}")]
        public async Task<AllTodosView> GetTodos([FromRoute] string userId, [FromQuery] bool? active)
        {
            var results = new List<TodoModel>();
            var aggregate = await _eventSourcingHandler.LoadAggregateByIdAsync(userId);
            if (aggregate == null || aggregate.Version == -1)
                return new AllTodosView();

            var view = _viewProjector.ProjectView<AllTodosView>(aggregate);

            if (active == null)
                return view;
            else
            {
                view.Todos.RemoveAll(x => x.Active != active);
                return view;
            }
        }

        [HttpPost("{userId}/{todoId}")]
        public async Task<IActionResult> UpdateTodo([FromRoute] string userId, [FromRoute] string todoId, [FromBody] UpdateTodoRequest request)
        {
            TodoAggregate aggregate;
            try
            {
                aggregate = await _eventSourcingHandler.LoadAggregateByIdAsync(userId);
                aggregate.UpdateTodo(todoId, request.Data, request.Active, DateTime.UtcNow);
                await _eventSourcingHandler.SaveAsync(aggregate);
            }
            catch (Exception ex) when (ex is AggregateException
                                          or AggregateNotFoundException
                                          or InvalidOperationException)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}