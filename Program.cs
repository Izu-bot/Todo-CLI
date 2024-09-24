using System.CommandLine;
using todo.src.commands;

namespace todo;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand("App gerenciador de lista de tarefas");

        rootCommand.AddCommand(new CreateCommand());

        return await rootCommand.InvokeAsync(args);
    }
}