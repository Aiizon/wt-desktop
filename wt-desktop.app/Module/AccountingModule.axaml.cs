using System.Windows.Input;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using wt_desktop.app.Controls;
using wt_desktop.app.Core;

namespace wt_desktop.app.Module;

public partial class AccountingModule : BaseModule
{
    public AccountingModule()
    {
        InitializeComponent();

        DataContext = this;
    }
}