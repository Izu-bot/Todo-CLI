using System.CommandLine;
using todo.src.commands;

namespace todo;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand("App gerenciador de lista de tarefas");

        rootCommand.AddCommand(new CreateCommand());
        rootCommand.AddCommand(new ListCommand());
        rootCommand.AddCommand(new DeleteCommand());
        rootCommand.AddCommand(new UpdateCommand());

        return await rootCommand.InvokeAsync(args);
    }
}