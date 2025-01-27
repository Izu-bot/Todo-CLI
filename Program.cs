using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using todo.src.data.repository;
using todo.src.commands;
using todo.src.services;
using todo.src.data;
using Microsoft.EntityFrameworkCore;

namespace todo;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var serviceColletion = new ServiceCollection();
        ConfigureServices(serviceColletion);

        var serviceProvider = serviceColletion.BuildServiceProvider();

        var rootCommand = new RootCommand("To-do list manager app");

        rootCommand.AddCommand(serviceProvider.GetRequiredService<CreateCommand>());
        rootCommand.AddCommand(serviceProvider.GetRequiredService<ListCommand>());
        rootCommand.AddCommand(serviceProvider.GetRequiredService<DeleteCommand>());
        rootCommand.AddCommand(serviceProvider.GetRequiredService<UpdateCommand>());

        return await rootCommand.InvokeAsync(args);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlite("Data Source=todo.db");
        });

        // Registrar serviços
        services.AddSingleton<ITodoRepository, TodoRepository>();
        services.AddSingleton<ITodoService, TodoService>();

        // Registrar os comandos
        services.AddTransient<CreateCommand>();
        services.AddTransient<ListCommand>();
        services.AddTransient<DeleteCommand>();
        services.AddTransient<UpdateCommand>();
    }
}