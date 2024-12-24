using Microsoft.EntityFrameworkCore;
using todo.src.model;

namespace todo.data;

public class ApplicationDbContext : DbContext
{
    // Propriedade anulavel
    public DbSet<Todo>? Todos { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=todo.db");
    }
}
