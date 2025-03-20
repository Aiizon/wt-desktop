using Avalonia.Controls;
using wt_desktop.app.Core.Controls;

namespace wt_desktop.app.Core.Module;

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
                PageContent.Content = new BayBoard(EBoardMode.Search, "");
                break;
            default:
                break;
        }
    }
}