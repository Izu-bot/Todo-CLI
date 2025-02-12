using System.CommandLine;
using System.Drawing;
using todo.src.services;
using todo.src.utils;

namespace todo.src.commands;

public class DeleteCommand : Command
{
    private readonly ITodoService _service;
    public DeleteCommand(ITodoService service)
        : base("remove", "Removes a task from the task list")
    {
        _service = service;
        ConsoleKeyInfo cki;

        var idArgument = new Argument<int>("id", "Provide the task id"); // Cria um novo argumento
        idArgument.AddValidator(result =>
        {
            try
            {
                // Verifica se o numero pesquisado é menor que 1
                if (result.GetValueOrDefault<int>() < 1)
                    result.ErrorMessage = "The id cannot be 0 or a negative number";
            }
            catch (Exception)
            {
                result.ErrorMessage = "Enter a valid number";
            }

        });
        AddArgument(idArgument);

        this.SetHandler((int id) =>
        {
            ColorConsole.HighlightMessage("Are you sure you want to remove the task?\ny(yes) n(no)", ConsoleColor.Yellow);
            
            do
            {
                cki = Console.ReadKey(true);
                if (cki.Key != ConsoleKey.Y && cki.Key != ConsoleKey.N)
                {
                    ColorConsole.HighlightMessage("\nInvalid input. Please press 'Y' (Yes) or 'N' (No).", ConsoleColor.Yellow);
                }
            }
            while (cki.Key != ConsoleKey.Y && cki.Key != ConsoleKey.N);


            if (cki.Key == ConsoleKey.Y)
            {
                ColorConsole.HighlightMessage($"\nTask {id} was removed successfully", ConsoleColor.Green);
                _service.DeleteTodoAsync(id);
                ViewList.ViewListDetail(_service.GetAllAsync().Result);
            }
            else
            {
                ColorConsole.HighlightMessage($"Task {id} has not been removed", ConsoleColor.Yellow);
            }

        }, idArgument);
    }
}