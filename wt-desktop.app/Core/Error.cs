using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using wt_desktop.tools;

namespace wt_desktop.app.Core;

/// <summary>
/// Représente une erreur à afficher dans l'application.
/// </summary>
public struct Error
{
    public string    Title     { get; set; }
    public string    Message   { get; set; }
    public string?   Code      { get; set; }
    public Exception Exception { get; set; }
    
    public Error
    (
        string    title, 
        string    message, 
        Exception exception,
        string?   code      = null
    )
    {
        Title     = title;
        Message   = message;
        Code      = code;
        Exception = exception;
    }
    
    /// <summary>
    /// Transforme l'erreur en chaîne JSON.
    /// </summary>
    /// <returns></returns>
    public string ToJsonString()
    {
        return $"{{" +
               $"\"title\":\"{Title}\"," +
               $"\"message\":\"{Message}\"," +
               $"\"exceptionDetails\":{JsonSerializer.Serialize(SerializeExceptionDetails())}," +
               $"\"code\":\"{Code}\"" +
               $"}}";
    }

    /// <summary>
    /// Transforme une chaîne JSON en erreur.
    /// </summary>
    /// <param name="jsonString">Chaîne</param>
    /// <returns>Erreur</returns>
    public static Error FromJson(string jsonString)
    {
        var json = JsonNode.Parse(jsonString)!.AsObject();
        return new Error
        (
            json["title"]  !.ToString(),
            json["message"]!.ToString(),
            new Exception(FormatExceptionDetails(json["exceptionDetails"])),
            json["code"]   !.ToString()
        );
    }
    
    /// <summary>
    /// Sérialise les détails de l'exception en un objet JSON.
    /// </summary>
    /// <returns>Détails</returns>
    private object SerializeExceptionDetails()
    {
        var details = new ExceptionDetails(
            Exception.GetType().ToString(),
            Exception.Message,
            Exception.StackTrace ?? string.Empty,
            Exception.Source     ?? string.Empty,
            new List<ExceptionDetails>()
        );

        var innerException = Exception.InnerException;
        while (innerException != null)
        {
            details.InnerExceptions!.Add
            (
                new ExceptionDetails
                (
                    innerException.GetType().ToString(),
                    innerException.Message,
                    innerException.StackTrace ?? string.Empty,
                    innerException.Source     ?? string.Empty
                )
            );

            innerException = innerException.InnerException;
        }

        return details;
    }
    
    /// <summary>
    /// Formate les détails de l'exception en une chaîne lisible.
    /// </summary>
    /// <param name="exceptionDetailsNode">Nœud JSON des détails</param>
    private static string? FormatExceptionDetails(JsonNode? exceptionDetailsNode)
    {
        if (exceptionDetailsNode == null)
        {
            return null;
        }
        
        var details = new System.Text.StringBuilder();

        details.AppendLine($"Exception type: {exceptionDetailsNode["Type"]}");
        details.AppendLine($"Message: {exceptionDetailsNode["Message"]}");

        if (!string.IsNullOrEmpty(exceptionDetailsNode["StackTrace"]?.ToString()))
        {
            details.AppendLine($"Stack trace: {exceptionDetailsNode["StackTrace"]}");
        }
        if (!string.IsNullOrEmpty(exceptionDetailsNode["Source"]?.ToString()))
        {
            details.AppendLine($"Source: {exceptionDetailsNode["Source"]}");
        }

        if (exceptionDetailsNode["InnerExceptions"]!.AsArray().Count == 0)
        {
            return details.ToString();
        }
        
        details.AppendLine("--- Inner exceptions ---");
        foreach (var innerException in exceptionDetailsNode["InnerExceptions"]!.AsArray())
        {
            details.AppendLine(FormatExceptionDetails(innerException));
        }

        return details.ToString();
    }
}

/// <summary>
/// Représente les détails d'une exception.
/// </summary>
internal struct ExceptionDetails
{
    public string                  Type            { get; set; }
    public string                  Message         { get; set; }
    public string                  StackTrace      { get; set; }
    public string                  Source          { get; set; }
    public List<ExceptionDetails>? InnerExceptions { get; set; }
    
    public ExceptionDetails
    (
        string                  type,
        string                  message,
        string                  stackTrace,
        string                  source,
        List<ExceptionDetails>? innerExceptions = null
    )
    {
        Type            = type;
        Message         = message;
        StackTrace      = stackTrace;
        Source          = source;
        InnerExceptions = innerExceptions;
    }
}