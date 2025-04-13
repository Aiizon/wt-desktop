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
        base.OnFrameworkInitializationCompleted();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            ErrorHandler.Initialize(
                error =>
                {
                    // Affiche l'erreur dans une popup et attend qu'elle soit fermée
                    var waitHandle = new ManualResetEvent(false);

                    var errorWindow = new ErrorWindow(error);
            
                    errorWindow.Closed += (_, _) => waitHandle.Set();
                    errorWindow.Show();
            
                    waitHandle.WaitOne();
                }
            );
            
            Task.Run(async () => await InitializeDatabaseAsync()).Wait();
            
            desktop.MainWindow = new LoginWindow();
            
            throw new Exception("test");
        }
        else
        {
            throw new Exception("Type d'application non supportée.");
        }
    }
    
    private async Task InitializeDatabaseAsync()
    {
        try
        {
            bool canConnect = await WtContext.Instance.Database.CanConnectAsync();
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