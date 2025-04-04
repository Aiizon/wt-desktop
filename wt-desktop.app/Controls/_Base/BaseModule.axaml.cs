using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using wt_desktop.app.Core;
using wt_desktop.ef;

namespace wt_desktop.app.Controls;

public partial class BaseModule : UserControl
{
    private Window? MainWindow    { get; }
    
    public ICommand LogoutCommand { get; }
    public ICommand ExitCommand   { get; }

    public static readonly StyledProperty<object> NavContentProperty =
        AvaloniaProperty.Register<BaseBoard, object>(nameof(NavContent));
    
    public static readonly StyledProperty<object> PageContentProperty =
        AvaloniaProperty.Register<BaseBoard, object>(nameof(PageContent));
    
    public object NavContent
    {
        get => GetValue(NavContentProperty);
        set => SetValue(NavContentProperty, value);
    }
    
    public object PageContent
    {
        get => GetValue(PageContentProperty);
        set => SetValue(PageContentProperty, value);
    }
    
    public BaseModule()
    {
        InitializeComponent();
        
        MainWindow = Avalonia.Application.Current!.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : throw new Exception("MainWindow is null");
        
        LogoutCommand = new RelayCommand(
            () =>
            {
                AuthProvider.Instance.Logout();
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                MainWindow!.Close();
            }, 
            () => AuthProvider.Instance.IsAuthenticated);
        
        ExitCommand = new RelayCommand(
            () =>
            {
                Environment.Exit(0);
            },
            () => AuthProvider.Instance.IsAuthenticated);
    }
}