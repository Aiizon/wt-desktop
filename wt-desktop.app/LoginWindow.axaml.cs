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

public partial class LoginWindow : SukiWindow
{
    public LoginWindow()
    {
        InitializeComponent();
        
        DataContext = new LoginWindowManager(this);
    }
}

public class LoginWindowManager: INotifyPropertyChanged
{
    #region Properties
    private readonly LoginWindow _Window;
    
    private string _Email;
    public string Email
    {
        get => _Email;
        set
        {
            _Email = value;
            OnPropertyChanged();
        }
    }

    private string _Password;
    public string Password
    {
        get => _Password;
        set
        {
            _Password = value;
            OnPropertyChanged();
        }
    }
    
    private string _ErrorMessage;
    public string ErrorMessage
    {
        get => _ErrorMessage;
        set
        {
            _ErrorMessage = value;
            OnPropertyChanged();
        }
    }
    #endregion
    
    public ICommand LoginCommand { get; }

    public LoginWindowManager(LoginWindow window)
    {
        _Window = window;
        
        LoginCommand = new RelayCommand(Login, () => true);
    }
    
    private void Login()
    {
        try
        {
            AuthProvider.Instance.Login(Email, Password);
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
            return;
        }

        var mainWindow = new MainWindow();
        
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = mainWindow;
            mainWindow.Show();
            _Window.Close();
        }
        else
        {
            throw new Exception("Unable to get the main window");
        }
    }
    
    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName]string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}