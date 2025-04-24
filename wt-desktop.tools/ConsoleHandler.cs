namespace wt_desktop.tools;

public static class ConsoleHandler
{
    private static void WriteLine(string message)
    {
        Console.WriteLine(message);
        Logger.WriteLog(message);
    }

    private static void Write(string message)
    {
        Console.Write(message);
        Logger.WriteLog(message);
    }

    public static void Clear()
    {
        Console.Clear();
    }
    
    public static void WriteError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        WriteLine(message);
        Logger.WriteLog(message);
        Console.ResetColor();
    }
    
    public static void WriteWarning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        WriteLine(message);
        Logger.WriteLog(message);
        Console.ResetColor();
    }
    
    public static void WriteSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        WriteLine(message);
        Logger.WriteLog(message);
        Console.ResetColor();
    }
    
    public static void WriteInfo(string message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        WriteLine(message);
        Logger.WriteLog(message);
        Console.ResetColor();
    }
    
    public static void WriteDebug(string message)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        WriteLine(message);
        Logger.WriteLog(message);
        Console.ResetColor();
    }
}