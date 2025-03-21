using SukiUI.Controls;

namespace wt_desktop.app.Core;

public partial class DeletionConfirmationDialog: SukiWindow
{
    public DeletionConfirmationDialog(DeletionConfirmationDialogManager manager)
    {
        InitializeComponent();

        DataContext = manager;
    }
}