using System;
using System.IO;
using System.IO.Pipes;

namespace wt_desktop.app.Core;

public static class ErrorHandler
{
    #region Constants
    private  static readonly string DefaultTitle   = "Erreur";
    private  static readonly string DefaultMessage = "Une erreur s'est produite. Veuillez réessayer.";
    private  static readonly string PipeName       = "wt-desktop-error-pipe";
    internal static readonly string ErrorArgument  = "--error-file=";
    #endregion
    
    /// <summary>
    /// Transforme une exception en erreur.
    /// </summary>
    /// <param name="exception">Exception</param>
    public static Error ProcessException(Exception exception)
    {
        return new Error(
            DefaultTitle,
            GetUserFriendlyErrorMessage(exception),
            null,
            exception
        );
    }
    
    /// <summary>
    /// Récupère un message d'erreur convivial basé sur le type d'exception.
    /// </summary>
    /// <param name="exception">Exception</param>
    /// <returns>Message d'erreur</returns>
    private static string GetUserFriendlyErrorMessage(Exception exception)
    {
        switch (typeof(Exception))
        {
            case var _ when exception is ArgumentNullException:
                return "Un argument requis est manquant.";
            case var _ when exception is ArgumentOutOfRangeException:
                return "Un argument est en dehors de la plage autorisée.";
            case var _ when exception is InvalidOperationException:
                return "L'opération demandée n'est pas valide dans l'état actuel.";
            case var _ when exception is NullReferenceException:
                return "Une référence null a été rencontrée.";
            case var _ when exception is OutOfMemoryException:
                return "La mémoire disponible est insuffisante.";
            default:
                return DefaultMessage;
        }
    }
    
    internal static void ReportErrorToLauncher(Error error)
    {
        try
        {
            var errorJsonString = error.ToJsonString();
            
            using var pipeClient = new NamedPipeClientStream(".", PipeName, PipeDirection.Out);
            
            Console.WriteLine($"Connexion au pipe {PipeName}...");

            try
            {
                pipeClient.Connect(1000);
                Console.WriteLine($"Connexion au pipe {PipeName} envoyée.");
                
                using var writer = new StreamWriter(pipeClient);
                writer.AutoFlush = true;
                
                Console.WriteLine($"Envoi de l'erreur au pipe {PipeName} : {errorJsonString}");
                writer.WriteLine(errorJsonString);

            }
            catch (Exception e)
            {
                Console.WriteLine("Impossible de se connecter au pipe.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erreur lors de l'envoi de l'erreur au pipe : {e.Message}");
        }
    }

    internal static void ReportErrorToLauncher(Exception exception)
        => ReportErrorToLauncher(ProcessException(exception));
}