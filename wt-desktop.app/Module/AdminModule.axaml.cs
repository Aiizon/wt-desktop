using Avalonia.Controls;

namespace wt_desktop.app.Module;

public partial class AdminModule : UserControl
{
    public AdminModule()
    {
        InitializeComponent();

        BayButton.Click += OnButtonClick;
    }

    private void OnButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Button button = (Button)sender!;

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