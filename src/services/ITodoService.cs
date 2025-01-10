using todo.ErrorManagement;
using todo.src.model;

namespace todo.src.services;

public interface ITodoService
{
    IQueryable<Todo> GetAll();
    (OperationsStatus, IQueryable<Todo>) GetTitle(string title);
    (OperationsStatus, Todo?) GetId(int id);
    OperationsStatus AddTodo(Todo todo);
    OperationsStatus UpdateTodo(Todo todo);
    OperationsStatus DeleteTodo(int id);
}
