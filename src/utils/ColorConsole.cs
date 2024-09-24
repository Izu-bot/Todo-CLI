namespace todo.src.utils;

public class ColorConsole
{
    public static void HighlightMessage(String message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}