using todo.src.model;

namespace todo.src.services;

public interface ITodoService
{
    IQueryable<Todo> GetAll();
    IQueryable? GetTitle(string title);
    Todo? GetId(int id);
    void AddTodo(Todo todo);
    void UpdateTodo(Todo todo);
    void DeleteTodo(int id);
}
