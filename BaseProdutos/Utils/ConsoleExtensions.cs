namespace BaseProdutos.Utils;

public static class ConsoleExtensions
{
    public static void ShowError(string message)
    {
        var oldForegroundColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ForegroundColor = oldForegroundColor;
    }
}