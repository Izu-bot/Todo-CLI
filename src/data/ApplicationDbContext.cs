using Microsoft.EntityFrameworkCore;
using todo.src.model;

namespace todo.src.data;

public class ApplicationDbContext : DbContext
{
    // Propriedade anulavel
    public required DbSet<Todo> Todos { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        // Inicializa a propriedade Todos no contrutor
        Todos = Set<Todo>();
        Database.Migrate(); // Garante que as migrações sejam aplicadas
    }
}
