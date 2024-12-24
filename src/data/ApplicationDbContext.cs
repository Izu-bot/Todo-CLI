using Microsoft.EntityFrameworkCore;
using todo.src.model;

namespace todo.data;

public class ApplicationDbContext : DbContext
{
    // Propriedade anulavel
    public required DbSet<Todo> Todos { get; set; }
    
    public ApplicationDbContext()
    {
        // Inicializa a propriedade Todos no contrutor
        Todos = Set<Todo>();
        Database.Migrate(); // Garante que as migrações sejam aplicadas
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=todo.db");
    }
}
