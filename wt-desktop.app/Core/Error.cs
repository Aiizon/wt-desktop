using System;
using System.Text.Json.Nodes;

namespace wt_desktop.app.Core;

/// <summary>
/// Représente une erreur à afficher dans l'application.
/// </summary>
public struct Error
{
    public string     Title     { get; set; }
    public string     Message   { get; set; }
    public string?    Code      { get; set; }
    public Exception? Exception { get; set; }
    
    public Error
    (
        string     title, 
        string     message, 
        string?    code      = null, 
        Exception? exception = null
    )
    {
        Title     = title;
        Message   = message;
        Code      = code;
        Exception = exception;
    }
    
    public string ToJsonString()
    {
        return $"{{" +
               $"\"title\":\"{Title}\"," +
               $"\"message\":\"{Message}\"," +
               $"\"code\":\"{Code}\"," +
               $"\"exceptionMessage\":\"{Exception!.Message}\"" +
               $"}}";
    }

    public static Error FromJson(string jsonString)
    {
        var json = JsonNode.Parse(jsonString)!.AsObject();
        return new Error
        (
            json["title"]  !.ToString(),
            json["message"]!.ToString(),
            json["code"]   !.ToString(),
            new Exception(json["exceptionMessage"]!.ToString())
        );
    }
}