using WebApi.Example.Todo.Models;

namespace WebApi.Example.Todo.MaterialisedViews
{
    public class AllTodosView
    {
        public List<TodoModel> Todos { get; set; } = new List<TodoModel>();
    }
}