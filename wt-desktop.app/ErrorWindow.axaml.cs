using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Avalonia;
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
    private ErrorWindow _Window;
    
    private Error       _Error;
    public  string      Title            => _Error.Title;
    public  string      Message          => _Error.Message;
    
    public  string      Code             => _Error.Code ?? string.Empty;
    public  bool        HasCode          => !string.IsNullOrEmpty(Code);
    public  string      FormattedCode    => HasCode ? string.Empty : $"Code d'erreur : {Code}";
    
    public  Exception?  Exception        => _Error.Exception;
    public  bool        HasException     => Exception != null;
    public  string      ExceptionDetails => HasException ? Exception!.Message : string.Empty;
    #endregion
    
    public ICommand CloseCommand           { get; }
    public ICommand CopyToClipboardCommand { get; }
    
    public ErrorWindowManager(Error error, ErrorWindow window)
    {
        _Error  = error;
        _Window = window;
        
        CloseCommand           = new RelayCommand(Close          , () => true);
        CopyToClipboardCommand = new RelayCommand(CopyToClipboard, () => true);
    }
    
    private void Close()
    {
        _Window.Close();
    }

    private async void CopyToClipboard()
    {
        await _Window.Clipboard!.SetTextAsync(_Error.Exception.Message);
    }
    
    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName]string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}