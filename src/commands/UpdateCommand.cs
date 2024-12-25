using System.CommandLine;
using System.Text.Json;
using System.Text.Json.Serialization;
using SQLitePCL;
using todo.src.model;
using todo.src.services;
using todo.src.utils;

namespace todo.src.commands;

public class UpdateCommand : Command
{
    private readonly ITodoService _service;
    public UpdateCommand(ITodoService service)
        : base("update", "Update a task")
    {
        _service = service;

        var idArgument = new Argument<int>("id", "The ID of the task you want to change");

        var titleOption = new Option<string>(
            name: "--title",
            description: "Can be used to change the task title"
            );

        titleOption.AddAlias("-t");
        this.AddOption(titleOption);

        var doneOption = new Option<string>(
            name: "--done",
            description: "Can be used to mark a task as completed"
        )
        {
            IsRequired = true
        };
        doneOption.AddAlias("-d");
        this.AddOption(doneOption);

        this.AddArgument(idArgument);

        this.SetHandler((int id, string title, string done) =>
        {
            // Procura a tarefa pelo ID
            var searchTodo = _service.GetId(id) ?? throw new InvalidOperationException($"Task with ID {id} not found.");

            // Atualizar a propriedade done e converdone para um booleano
            if (!string.IsNullOrWhiteSpace(done)) searchTodo!.IsDone = done.Equals("true", StringComparison.OrdinalIgnoreCase);

            // Atualiza title se não for vazio
            if (!string.IsNullOrWhiteSpace(title)) searchTodo!.Title = title;

            // Chama o serviço para persistir no banco
            _service.UpdateTodo(searchTodo!);

            ColorConsole.HighlightMessage(
                $"Task successfully updated!\nNew Task: ID: {searchTodo.Id} Title: {searchTodo.Title}, Done: {searchTodo.IsDone}, Created At: {searchTodo.CreatedAt}"
                , ConsoleColor.Green
            );

        }, idArgument, titleOption, doneOption);
    }
}


