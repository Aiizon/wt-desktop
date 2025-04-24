using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using wt_desktop.app.Accounting;
using wt_desktop.app.Controls;
using wt_desktop.app.Core;

namespace wt_desktop.app.Module;

public partial class AccountingModule : BaseModule
{
    public ICommand NavigateToCustomerCommand { get; }
    public ICommand NavigateToRentalCommand { get; }
    
    public AccountingModule()
    {
        InitializeComponent();
        
        NavigateToCustomerCommand = new RelayCommand(() => PageContent = new CustomerBoard(EBoardMode.Search, ""), () => true);
        NavigateToRentalCommand   = new RelayCommand(() => PageContent = new RentalBoard  (EBoardMode.Search, ""), () => true);

        DataContext = this;
    }
}