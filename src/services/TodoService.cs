using todo.data.repository;
using todo.src.model;
using todo.src.services;

namespace todo.src.services;

public class TodoService(TodoRepository repository) : ITodoService
{
    private readonly ITodoRepository _repository = repository;

    public void AddTodo(Todo todo) => _repository.AddTodo(todo);

    public void DeleteTodo(int id)
    {
        var todo = _repository.GetId(id);

        if (todo != null) _repository.DeleteTodo(todo);
    }

    public IQueryable<Todo> GetAll() => _repository.GetAll();

    public Todo? GetId(int id) => _repository.GetId(id);

    public IQueryable? GetTitle(string title) => _repository.GetTitle(title);

    public void UpdateTodo(Todo todo) => _repository.UpdateTodo(todo);
}
