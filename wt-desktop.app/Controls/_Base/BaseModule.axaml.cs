using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using wt_desktop.ef;

namespace wt_desktop.app.Controls;

public partial class BaseModule : UserControl
{
    public ICommand LogoutCommand { get; }
    public ICommand ExitCommand   { get; }

    public static readonly StyledProperty<object> NavContentProperty =
        AvaloniaProperty.Register<BaseModule, object>(nameof(NavContent));
    
    public static readonly StyledProperty<object> PageContentProperty =
        AvaloniaProperty.Register<BaseModule, object>(nameof(PageContent));
    
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
    
    protected BaseModule()
    {
        InitializeComponent();
        
        if(Application.Current!.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
        {
            LogoutCommand = new RelayCommand(
                () =>
                {
                    // L'exit code 2 est utilisé pour indiquer que l'utilisateur a été déconnecté
                    // L'application redémarre et affiche la page de login
                    Environment.Exit(2);
                },
                () => true);

            ExitCommand = new RelayCommand(
                () =>
                {
                    Environment.Exit(0);
                },
                () => true);
        }
        else
        {
            throw new Exception("Le support n'est pas pris en charge.");
        }
    }
}