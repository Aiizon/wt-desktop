using Avalonia.Controls;
using Avalonia.Interactivity;

namespace wt_desktop.app;

public partial class BayBoard : UserControl
{
    public BayModel BayModel { get; }

    public BayBoard()
    {
        InitializeComponent();

        DataContext = BayModel = new BayModel();

        SearchButton.Click += OnButtonClick;
    }

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        Button button = (Button)sender!;

        switch (button.Name)
        {
            case "SearchButton":
                BayModel.ReloadSource();
                break;
            case "SaveButton":
                BayModel.AddEntity();
                break;
            default:
                break;
        }
    }
}