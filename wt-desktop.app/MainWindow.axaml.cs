using Avalonia.Controls;
using Avalonia.Interactivity;
using wt_desktop.app.Module;

namespace wt_desktop.app;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        AdminButton.Click      += OnButtonClick;
        AccountantButton.Click += OnButtonClick;
    }

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        Button button = (Button)sender!;

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