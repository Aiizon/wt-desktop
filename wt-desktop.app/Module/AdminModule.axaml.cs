using Avalonia.Controls;
using wt_desktop.app.Controls;

namespace wt_desktop.app.Module;

public partial class AdminModule : UserControl
{
    public AdminModule()
    {
        InitializeComponent();
    }

    private void OnButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        NavigationButton button = (NavigationButton)sender!;

        switch (button.Name)
        {
            case "BayButton":
                PageContent.Content = new BayBoard();
                break;
            default:
                break;
        }
    }
}