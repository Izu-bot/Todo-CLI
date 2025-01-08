using todo.ErrorManagement;
using todo.src.model;

namespace todo.src.services;

public interface ITodoService
{
    IQueryable<Todo> GetAll();
    (OperationsError, IQueryable<Todo>) GetTitle(string title);
    (OperationsError, Todo?) GetId(int id);
    OperationsError AddTodo(Todo todo);
    OperationsError UpdateTodo(Todo todo);
    OperationsError DeleteTodo(int id);
}
