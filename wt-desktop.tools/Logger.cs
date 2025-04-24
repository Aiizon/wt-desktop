using System.Globalization;

namespace wt_desktop.tools;

public static class Logger
{
    private static readonly string LogFile     = "log.txt";
    private static readonly string LogFilePath = Path.Combine(Environment.CurrentDirectory, LogFile);
    
    public static void WriteLog(string message)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(LogFilePath, true))
            {
                writer.WriteLine(Format(message));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'écriture dans le fichier de journalisation : {ex.Message}");
        }
    }
    
    private static string Format(string message)
    {
        return $"[LOG] {DateTime.Now.ToString(CultureInfo.CurrentCulture)} : {message}";
    }
}