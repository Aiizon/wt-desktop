using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;
using SukiUI.Controls;
using wt_desktop.app.Core;

namespace wt_desktop.app;

public partial class ErrorWindow : SukiWindow
{
    public ErrorWindow(Error error)
    {
        InitializeComponent();
        
        DataContext = new ErrorWindowManager(error);
    }
}

public class ErrorWindowManager
{
    #region Properties
    private Error      Error;
    public  string     Title            => Error.Title;
    public  string     Message          => Error.Message;
    
    public  string     Code             => Error.Code ?? string.Empty;
    public  bool       HasCode          => !string.IsNullOrEmpty(Code);
    public  string     FormattedCode    => HasCode ? string.Empty : $"Code d'erreur : {Code}";
    
    public  Exception? Exception        => Error.Exception;
    public  bool       HasException     => Exception != null;
    public  string     ExceptionDetails => HasException ? Exception!.Message : string.Empty;
    #endregion
    
    public ICommand CloseCommand { get; }
    
    public ErrorWindowManager(Error error)
    {
        Error = error;
        
        CloseCommand = new RelayCommand(Close, () => true);
    }
    
    private void Close()
    {
        if (Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
        else
        {
            throw new Exception("Type d'application non supportée.");
        }
    }
}