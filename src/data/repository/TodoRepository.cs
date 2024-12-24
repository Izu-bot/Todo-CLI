using Microsoft.EntityFrameworkCore;
using todo.src.model;

namespace todo.data.repository;

public class TodoRepository : ITodoRepository
{
    private readonly ApplicationDbContext _context;
    public TodoRepository(ApplicationDbContext context) => _context = context;
    public void AddTodo(Todo todo)
    {
        _context.Todos.Add(todo);
        _context.SaveChanges();
    }

    public void DeleteTodo(Todo todo)
    {
        _context.Todos.Remove(todo);
        _context.SaveChanges();
    }

    public IQueryable<Todo> GetAll() => _context.Todos.AsQueryable();

    public Todo? GetId(int id) => _context.Todos.Single(i => i.Id == id);

    public IQueryable? GetTitle(string title) => _context.Todos.Where(n => n.Title == title).AsNoTracking();

    public void UpdateTodo(Todo todo)
    {
        _context.Todos.Update(todo);
        _context.SaveChanges();
    }
}
