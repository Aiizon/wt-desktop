using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using SukiUI;
using SukiUI.Enums;
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
        Task.Run(async () => await InitializeDatabaseAsync()).Wait();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
    
    private async Task InitializeDatabaseAsync()
    {
        try
        {
            bool canConnect = await WtContext.Instance.Database.CanConnectAsync();
            if (!canConnect)
            {
                throw new Exception("Database connection failed.");
            }
            var units = WtContext.Instance.Unit.ToList();
            var bays = WtContext.Instance.Bay.ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}