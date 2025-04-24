using System.Globalization;

namespace wt_desktop.tools;

public static class Logger
{
    private static readonly string LogFile     = "wt-desktop.log";
    private static readonly string LogFilePath = Path.Combine(Environment.CurrentDirectory, LogFile);

    public static void Initialize()
    {
        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        {
            LogException((e.ExceptionObject as Exception)!);
        };
        
        TaskScheduler.UnobservedTaskException += (_, e) =>
        {
            LogException(e.Exception);
        };
        
        AppDomain.CurrentDomain.FirstChanceException += (_, e) =>
        {
            LogException(e.Exception);
        };
    }
    
    public static void WriteLog(string message)
    {
        try
        {
            using StreamWriter writer = new StreamWriter(LogFilePath, true);
            writer.WriteLine(Format(message, "LOG"));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'écriture dans le fichier de journalisation : {ex.Message}");
        }
    }
    
    public static void LogException(Exception ex)
    {
        try
        {
            using StreamWriter writer = new StreamWriter(LogFilePath, true);
            writer.WriteLine(Format($"{ex.GetType().Name} - {ex.Message}", "EXCEPTION"));;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erreur lors de l'écriture dans le fichier de journalisation : {e.Message}");
        }
    }
    
    private static string Format(string message, string type)
    {
        return $"[{type}] {DateTime.Now.ToString(CultureInfo.CurrentCulture)} : {message}";
    }
}