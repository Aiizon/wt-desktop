using System;
using System.IO;
using System.IO.Pipes;
using wt_desktop.tools;

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
            case var _ when exception is IOException:
                return "Une erreur d'entrée/sortie s'est produite.";
            case var _ when exception is UnauthorizedAccessException:
                return "Accès non autorisé à une ressource.";
            case var _ when exception is FileNotFoundException:
                return "Le fichier spécifié est introuvable.";
            case var _ when exception is DirectoryNotFoundException:
                return "Le répertoire spécifié est introuvable.";
            case var _ when exception is NoLauncherException:
                return "Veuillez lancer l'application via le launcher.";
            default:
                return DefaultMessage;
        }
    }
    
    private static void ReportErrorToLauncher(Error error)
    {
        try
        {
            var errorJsonString = error.ToJsonString();
            
            using var pipeClient = new NamedPipeClientStream(".", PipeName, PipeDirection.Out);
            
            ConsoleHandler.WriteInfo($"Connexion au pipe {PipeName}...");

            try
            {
                pipeClient.Connect(1000);
                ConsoleHandler.WriteInfo($"Connexion au pipe {PipeName} envoyée.");
                
                using var writer = new StreamWriter(pipeClient);
                writer.AutoFlush = true;
                
                ConsoleHandler.WriteDebug($"Envoi de l'erreur au pipe {PipeName} : {errorJsonString}");
                writer.WriteLine(errorJsonString);

            }
            catch
            {
                ConsoleHandler.WriteError("Impossible de se connecter au pipe.");
            }
        }
        catch (Exception e)
        {
            ConsoleHandler.WriteError($"Erreur lors de l'envoi de l'erreur au pipe : {e.Message}");
        }
    }

    internal static void ReportErrorToLauncher(Exception exception)
        => ReportErrorToLauncher(ProcessException(exception));
}