using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;
using SukiUI.Controls;
using wt_desktop.ef;

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
    private readonly LoginWindow _window;
    
    private string _email;
    public string  Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged();
        }
    }

    private string _password;
    public string  Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }
    
    private string _errorMessage;
    public string  ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }
    #endregion
    
    public ICommand LoginCommand { get; }

    public LoginWindowManager(LoginWindow window)
    {
        _window = window;
        
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
            _window.Close();
        }
        else
        {
            throw new Exception("Unable to get the main window");
        }
    }
    
    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName]string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}