using System.CommandLine;
using todo.src.model;
using todo.src.services;
using todo.src.utils;

namespace todo.src.commands;

public class ListCommand : Command
{
    private readonly ITodoService _service;
    public ListCommand(ITodoService service)
        : base("list", "List all taks")
    {
        _service = service;

        var titleOptions = new Option<string>(
            name: "--name",
            description: "An option to search by names"
        );
        titleOptions.AddAlias("-n");

        AddOption(titleOptions);

        this.SetHandler((name) =>
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                var todo = _service.GetAll(); 

                foreach (Todo item in todo)
                {
                    ColorConsole.HighlightMessage(
                        $"ID: {item.Id}\tTitle: {item.Title}\tStatus: {item.IsDone}\tCreated at: {item.CreatedAt}",
                        ConsoleColor.Blue
                    );
                }
            }
            else
            {
                var todo = _service.GetTitle(name);
                
                foreach (Todo item in todo)
                {
                    ColorConsole.HighlightMessage(
                        $"ID: {item.Id}\tTitle: {item.Title}\tStatus: {item.IsDone}\tCreated at: {item.CreatedAt}",
                        ConsoleColor.Blue
                    );
                }
            }

        }, titleOptions);
    }
}