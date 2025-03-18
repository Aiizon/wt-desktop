using Avalonia.Controls;
using Avalonia.Interactivity;
using wt_desktop.app.Controls;

namespace wt_desktop.app;

public partial class BayBoard : UserControl
{
    public BayModel BayModel { get; }

    public BayBoard()
    {
        InitializeComponent();

        DataContext = BayModel = new BayModel();
        BayModel.ReloadSource();
    }

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        UserControl button = (UserControl)sender!;

        switch (button.Name)
        {
            case "SearchButton":
                BayModel.ReloadSource();
                break;
            case "SaveButton":
                BayModel.SaveChanges();
                break;
            case "AddButton":
                BayModel.AddEntity();
                break;
            default:
                break;
        }
    }
}