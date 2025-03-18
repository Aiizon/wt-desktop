using Avalonia.Controls;
using Avalonia.Interactivity;
using SukiUI.Controls;
using wt_desktop.app.Controls;
using wt_desktop.app.Module;

namespace wt_desktop.app;

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