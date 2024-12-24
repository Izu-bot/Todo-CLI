using Microsoft.EntityFrameworkCore;

namespace todo.data;

public static class DatabaseInitializer
    {
        public static void Initialize()
        {
            // Se a propriedade Todos for "required" apresenta um erro CS9035
            var context = new ApplicationDbContext();
            context.Database.Migrate(); // Migrate garante que o esquema esteja atualizado
        }
    }
