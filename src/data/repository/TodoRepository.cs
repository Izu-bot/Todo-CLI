using Microsoft.EntityFrameworkCore;
using todo.src.model;

namespace todo.src.data.repository;

public class TodoRepository(ApplicationDbContext context) : ITodoRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task AddTodoAsync(Todo todo)
    {
        await _context.Todos.AddAsync(todo);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTodoAsync(Todo todo)
    {
        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync();
    }

    public IQueryable<Todo> GetAll()
    {
        return _context.Todos.AsQueryable();
    }

    public async Task<Todo?> GetIdAsync(int id)
    {
        return await _context.Todos.FindAsync(id);
    }

    public IQueryable<Todo> GetTitle(string title)
    {
        return _context.Todos.Where(t => t.Title!.Contains(title)); // t => t.Title.Contains(title)
    }

    public async Task UpdateTodoAsync(Todo todo)
    {
        _context.Todos.Update(todo);
        await _context.SaveChangesAsync();
    }
}
