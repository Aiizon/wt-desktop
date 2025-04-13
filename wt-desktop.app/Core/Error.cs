using System;
using System.Windows.Input;

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
}