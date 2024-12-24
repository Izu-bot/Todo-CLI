using System.CommandLine;
using todo.data;
using todo.src.commands;

namespace todo;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand("To-do list manager app");

        rootCommand.AddCommand(new CreateCommand());
        rootCommand.AddCommand(new ListCommand());
        rootCommand.AddCommand(new DeleteCommand());
        rootCommand.AddCommand(new UpdateCommand());

        return await rootCommand.InvokeAsync(args);
    }
}