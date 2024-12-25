using System.CommandLine;
using todo.src.model;
using todo.src.services;
using todo.src.utils;

namespace todo.src.commands;

public class CreateCommand : Command
{
    private readonly ITodoService _service;
    public CreateCommand(ITodoService service)
        : base("add", "Adds a new item to the list")
    {
        _service = service;

        // Adiciona um novo argumento ao comando
        var titleArgument = new Argument<string[]>("title", "Task title")
        {
            // Exige pelo menos um titulo
            Arity = ArgumentArity.OneOrMore
        };

        titleArgument.AddValidator(result =>
        {
            var titles = result.GetValueForArgument(titleArgument); // Captura a coleção de argumentos
            foreach (var title in titles) // Itera sobre cada titulo
            {
                if (string.IsNullOrWhiteSpace(title)) // Verifica se está vazio ou com espaços em branco
                {
                    result.ErrorMessage = "Titles must not be empty";
                    return;
                }
            }
        });
        AddArgument(titleArgument);

        // Diz o que o comando vai fazer ao ser chamado
        this.SetHandler((string[] titles) =>
        {
            foreach (var title in titles)
            {
                // Deixa a primeira letra maiuscula e as demais minuscula
                string formatter = title[0].ToString().ToUpper() + title[1..].ToLower().Replace(",", "");
                var newTodo = new Todo { Title = formatter };

                _service.AddTodo(newTodo); // Adicionar uma nova tarefa
                ColorConsole.HighlightMessage($"Title: {formatter}, Date: {newTodo.CreatedAt:d}", ConsoleColor.Green);
            }
            // Console.WriteLine("✅ Adicionado");
        }, titleArgument);
    }
}