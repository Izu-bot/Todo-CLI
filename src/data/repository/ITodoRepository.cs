using todo.src.model;

namespace todo.src.data.repository;

public interface ITodoRepository
{
    IQueryable<Todo> GetAll();
    IQueryable? GetTitle(string title);
    Todo? GetId(int id);
    void AddTodo(Todo todo);
    void UpdateTodo(Todo todo);
    void DeleteTodo(Todo todo);
}
