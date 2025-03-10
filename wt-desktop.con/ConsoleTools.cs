namespace wt_desktop.con;

public class ConsoleTools
{
    #region output
    public static void Pause(string message = ">", bool doClear = false)
    {
        Display(message, "", ConsoleColor.DarkGray, ConsoleColor.Gray);
        Console.ReadKey(true);
        if (doClear)
            Console.Clear();
    }

    public static void PrintError(Exception e)
    {
        Display(e.Message, e.GetType().FullName, ConsoleColor.Red, ConsoleColor.DarkRed);
    }

    public static void Display
    (
        IEnumerable<string> messages,
        string title = "",
        ConsoleColor titleColor   = ConsoleColor.Cyan,
        ConsoleColor messageColor = ConsoleColor.White
    )
    {
        if (!string.IsNullOrEmpty(title))
        {
            Console.ForegroundColor = titleColor;
            Console.WriteLine(title);
        }

        IEnumerable<string> parsedMessages = messages as string[] ?? messages.ToArray();
        if (parsedMessages.Any())
        {
            Console.ForegroundColor = messageColor;
            foreach (string message in parsedMessages)
                Console.WriteLine(message);
        }

        Console.ResetColor();
    }

    public static void Display
    (
        string message,
        string title = "",
        ConsoleColor titleColor   = ConsoleColor.Cyan,
        ConsoleColor messageColor = ConsoleColor.White
    )
    {
        Display([message], title, titleColor, messageColor);
    }
    #endregion
}