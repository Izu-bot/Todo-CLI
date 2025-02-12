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
            name: "--title",
            description: "An option to search by names"
        );
        titleOptions.AddAlias("-t");

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

        this.SetHandler(async (name, id) =>
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                if (id != 0)
                {
                    var todo = await _service.GetIdAsync(id);

                    if (todo != null) ViewList.ViewListDetail([todo]);
                    else ColorConsole.HighlightMessage($"Indicates that the resource was not found in your database. Check the Id number or Title passed in the search.", ConsoleColor.Red);
                }
                else
                {
                    var todos = await _service.GetAllAsync();

                    ViewList.ViewListDetail(todos);
                }
            }
            else
            {
               var todos = await _service.GetTitleAsync(name);

                if (todos != null)
                {
                    ViewList.ViewListDetail(todos);
                }
                else ColorConsole.HighlightMessage($"Indicates that the resource was not found in your database. Check the Id number or Title passed in the search.", ConsoleColor.Red);
            }
        }, titleOptions, idOptions);
    }
}