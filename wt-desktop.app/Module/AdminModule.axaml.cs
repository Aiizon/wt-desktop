using System.Windows.Input;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using wt_desktop.app.Admin;
using wt_desktop.app.Controls;
using wt_desktop.app.Core;

namespace wt_desktop.app.Module;

public partial class AdminModule : BaseModule
{
    public ICommand NavigateToBayCommand          { get; } 
    public ICommand NavigateToUnitCommand         { get; } 
    public ICommand NavigateToUserCommand         { get; } 
    public ICommand NavigateToOfferCommand         { get; } 
    public ICommand NavigateToInterventionCommand { get; } 

    public AdminModule()
    {
        InitializeComponent();

        NavigateToBayCommand          = new RelayCommand(() => PageContent = new BayBoard         (EBoardMode.Search, ""), () => true);
        NavigateToUnitCommand         = new RelayCommand(() => PageContent = new UnitBoard        (EBoardMode.Search, ""), () => true);
        NavigateToUserCommand         = new RelayCommand(() => PageContent = new UserBoard        (EBoardMode.Search, ""), () => true);
        NavigateToOfferCommand        = new RelayCommand(() => PageContent = new OfferBoard       (EBoardMode.Search, ""), () => true);
        NavigateToInterventionCommand = new RelayCommand(() => PageContent = new InterventionBoard(EBoardMode.Search, ""), () => true);

        DataContext = this;
    }
}