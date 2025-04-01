using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using wt_desktop.app.Accounting;
using wt_desktop.app.Controls;
using wt_desktop.app.Core;

namespace wt_desktop.app.Module;

public partial class AccountingModule : BaseModule
{
    public ICommand NavigateToUserCommand { get; }
    
    public AccountingModule()
    {
        InitializeComponent();
        
        NavigateToUserCommand = new RelayCommand(() => PageContent = new UserBoard(EBoardMode.Search, ""), () => true);

        DataContext = this;
    }
}