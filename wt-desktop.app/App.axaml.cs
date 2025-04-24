using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using SukiUI;
using SukiUI.Enums;
using wt_desktop.app.Core;
using wt_desktop.ef;
using wt_desktop.tools;

namespace wt_desktop.app;

public partial class App : Application
{
    private static readonly string AuthorizedArgument = "--authorized";

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
            var args          = desktop.Args;
            var errorFilePath = GetErrorFilePath(args);

            if (!args!.Contains(AuthorizedArgument))
            {
                desktop.MainWindow = new ErrorWindow
                (
                    ErrorHandler.ProcessException(new NoLauncherException("Veuillez lancer l'application via le launcher."))
                );
                
                return;
            }
            
            if (!string.IsNullOrEmpty(errorFilePath) && File.Exists(errorFilePath))
            {
                try
                {
                    string errorJsonString = File.ReadAllText(errorFilePath);
                    var    error           = Error.FromJson(errorJsonString);
                
                    // Affiche la fenêtre d'erreur
                    desktop.MainWindow = new ErrorWindow(error);
                    
                    File.Delete(errorFilePath);
                    
                    base.OnFrameworkInitializationCompleted();
                    return;
                }
                catch (Exception ex)
                {
                    ConsoleHandler.WriteError($"Erreur lors de la lecture du fichier d'erreur : {ex.Message}");
                }
            }
            
            AppDomain.CurrentDomain.UnhandledException += (_, e) =>
            {
                if (e.ExceptionObject is Exception ex)
                {
                    ErrorHandler.ReportErrorToLauncher(ex);
                }
            };
        
            TaskScheduler.UnobservedTaskException += (_, e) =>
            {
                e.SetObserved();
                ErrorHandler.ReportErrorToLauncher(e.Exception);
            };

            try
            {
                Task.Run(async () => await InitializeDatabaseAsync()).Wait();

                desktop.MainWindow = new LoginWindow();
                
                base.OnFrameworkInitializationCompleted();
            }
            catch (Exception e)
            {
                ErrorHandler.ReportErrorToLauncher(e);
            }
            
        }
        // else
        // {
        //     ErrorHandler.ReportErrorToLauncher(new Exception("Impossible de lancer l'application."));
        // }
    }
    
    private string? GetErrorFilePath(string[]? args)
    {
        if (args == null)
        {
            return null;
        }
        
        foreach (var arg in args)
        {
            if (arg.StartsWith(ErrorHandler.ErrorArgument))
            {
                return arg.Substring(ErrorHandler.ErrorArgument.Length).Trim('"');
            }
        }
    
        return null;
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
            ConsoleHandler.WriteError(e.Message);
            throw;
        }
    }
}