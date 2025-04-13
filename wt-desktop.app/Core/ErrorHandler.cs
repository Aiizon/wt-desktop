using System;
using System.Threading.Tasks;

namespace wt_desktop.app.Core;

public static class ErrorHandler
{
    #region Defaults
    private static readonly string DefaultTitle   = "Erreur";
    private static readonly string DefaultMessage = "Une erreur s'est produite. Veuillez réessayer.";
    #endregion
    
    private static Action<Error> _DisplayErrorAction;
    
    /// <summary>
    /// Initialise le gestionnaire d'erreurs.
    /// </summary>
    /// <param name="displayErrorAction">Action qui va afficher la popup d'erreur</param>
    public static void Initialize(Action<Error> displayErrorAction)
    {
        _DisplayErrorAction = displayErrorAction;
        
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        TaskScheduler.UnobservedTaskException      += OnUnobservedTaskException;
    }
    
    /// <summary>
    /// Affiche une erreur.
    /// </summary>
    /// <param name="title">Titre</param>
    /// <param name="message">Message</param>
    /// <param name="code">Code d'erreur</param>
    /// <param name="exception">Exception</param>
    public static void ShowError
    (
        string     title, 
        string     message, 
        string?    code      = null, 
        Exception? exception = null
    )
    {
        var error = new Error
        (
            title,
            message,
            code,
            exception
        );
        
        _DisplayErrorAction?.Invoke(error);
    }
    
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
    /// Traite une exception et l'affiche.
    /// </summary>
    /// <param name="exception">Exception</param>
    public static void HandleException(Exception exception)
    {
        ShowError(
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
                return "Une erreur inattendue s'est produite.";
        }
    }
    
    /// <summary>
    /// Traite une exception non observée.
    /// </summary>
    private static void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        e.SetObserved();
        HandleException(e.Exception);
    }
    
    /// <summary>
    /// Traite une exception non gérée.
    /// </summary>
    private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception exception)
        {
            HandleException(exception);
        }
    }
}