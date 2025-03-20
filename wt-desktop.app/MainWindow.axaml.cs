using Avalonia.Controls;
using Avalonia.Interactivity;
using SukiUI.Controls;
using wt_desktop.app.Core.Controls;
using wt_desktop.app.Core.Module;

namespace wt_desktop.app.Core;

public partial class MainWindow : SukiWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        ModuleButton button = (ModuleButton)sender!;

        switch (button.Name)
        {
            case "AdminButton":
                MainContent.Content = new AdminModule();
                break;
            case "AccountantButton":
                MainContent.Content = null;
                break;
            default:
                MainContent.Content = null;
                break;
        }
    }
}