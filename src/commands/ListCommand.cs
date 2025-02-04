using System.CommandLine;
using todo.ErrorManagement;
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

        var idOptions = new Option<int>(
            name: "--id",
            description: "An option to seach by id's"
        );

        idOptions.AddValidator(result =>
        {
            var value = result.Tokens.SingleOrDefault()?.Value;
            if (value is not null && !int.TryParse(value, out _))
                result.ErrorMessage = $"Invalid entry. The ID '{value}' provided is not valid.";
        });

        idOptions.AddAlias("-i");

        AddOption(titleOptions);
        AddOption(idOptions);

        this.SetHandler((name, id) =>
        {

            if (String.IsNullOrWhiteSpace(name))
            {
                if (id != 0)
                {
                    var (status, todo) = _service.GetId(id);

                    if (status.IsSuccess() && todo != null) ViewList.ViewListDetail(new List<Todo> { todo });
                    else ColorConsole.HighlightMessage($"Indicates that the resource was not found in your database. Check the Id number or Title passed in the search.", ConsoleColor.Red);

                }
                else
                {
                    var todos = _service.GetAll();

                    ViewList.ViewListDetail([.. todos]);
                }
            }
            else
            {
                var (status, todos) = _service.GetTitle(name);

                if (status.IsSuccess())
                {
                    ViewList.ViewListDetail([.. todos]);
                }
                else ColorConsole.HighlightMessage($"Indicates that the resource was not found in your database. Check the Id number or Title passed in the search.", ConsoleColor.Red);
            }

        }, titleOptions, idOptions);
    }
}