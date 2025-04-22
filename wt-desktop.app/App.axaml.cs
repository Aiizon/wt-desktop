using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using SukiUI;
using SukiUI.Enums;
using wt_desktop.app.Core;
using wt_desktop.ef;

namespace wt_desktop.app;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        SukiTheme.GetInstance().ChangeBaseTheme(ThemeVariant.Dark);
        SukiTheme.GetInstance().ChangeColorTheme(SukiColor.Blue);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            ErrorHandler.Initialize(
                error =>
                {
                    // Affiche l'erreur dans une popup et attend qu'elle soit fermée
                    var waitHandle  = new ManualResetEvent(false);

                    var errorWindow = new ErrorWindow(error);
            
                    errorWindow.Closed += (_, _) => waitHandle.Set();
                    desktop.MainWindow = errorWindow;
            
                    // waitHandle.WaitOne();
                }
            );
            
            AppDomain.CurrentDomain.UnhandledException += (_, e) =>
            {
                if (e.ExceptionObject is Exception ex)
                {
                    ErrorHandler.HandleException(ex);
                }
            };
            
            TaskScheduler.UnobservedTaskException += (_, e) =>
            {
                e.SetObserved();
                ErrorHandler.HandleException(e.Exception);
            };
            
            Task.Run(async () => await InitializeDatabaseAsync()).Wait();
            
            desktop.MainWindow = new LoginWindow();
        }
        else
        {
            throw new Exception("Type d'application non supportée.");
        }
        
        
        base.OnFrameworkInitializationCompleted();
        throw new Exception("test");
    }
    
    private async Task InitializeDatabaseAsync()
    {
        try
        {
            var canConnect = await WtContext.Instance.Database.CanConnectAsync();
            if (!canConnect)
            {
                throw new Exception("Impossible de se connecter à la base de données.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}