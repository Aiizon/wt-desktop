using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SukiUI.Controls;
using wt_desktop.app.Core;

namespace wt_desktop.app;

public partial class ErrorWindow : SukiWindow
{
    public ErrorWindow(Error error)
    {
        InitializeComponent();
        
        DataContext = new ErrorWindowManager(error, this);
    }
}

public class ErrorWindowManager: INotifyPropertyChanged
{
    #region Properties
    private readonly ErrorWindow _window;
    
    private readonly Error       _error;
    
    public  string     Title            => _error.Title;
    public  string     Message          => _error.Message;
    
    private string     Code             => _error.Code ?? string.Empty;
    public  bool       HasCode          => !string.IsNullOrEmpty(Code);
    public  string     FormattedCode    => HasCode ? string.Empty : $"Code d'erreur : {Code}";
    
    private Exception? Exception        => _error.Exception;
    public  bool       HasException     => Exception != null;
    public  string     ExceptionDetails => HasException ? Exception!.Message : string.Empty;
    #endregion
    
    public ICommand CloseCommand           { get; }
    public ICommand CopyToClipboardCommand { get; }
    
    public ErrorWindowManager(Error error, ErrorWindow window)
    {
        _error  = error;
        _window = window;
        
        CloseCommand           = new RelayCommand(Close          , () => true);
        CopyToClipboardCommand = new RelayCommand(CopyToClipboard, () => true);
    }
    
    private void Close()
    {
        _window.Close();
    }

    private void CopyToClipboard()
    {
        _window.Clipboard!.SetTextAsync(_error.Exception.Message);
    }
    
    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName]string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}