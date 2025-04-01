using System.Windows.Input;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using wt_desktop.app.Core;

namespace wt_desktop.app.Module;

public partial class AdminModule : UserControl
{
    public ICommand NavigateToBayCommand          { get; } 
    public ICommand NavigateToUnitCommand         { get; } 
    public ICommand NavigateToUserCommand         { get; } 
    public ICommand NavigateToOfferCommand         { get; } 
    public ICommand NavigateToInterventionCommand { get; } 

    public AdminModule()
    {
        InitializeComponent();

        NavigateToBayCommand          = new RelayCommand(() => PageContent.Content = new BayBoard         (EBoardMode.Search, ""), () => true);
        NavigateToUnitCommand         = new RelayCommand(() => PageContent.Content = new UnitBoard        (EBoardMode.Search, ""), () => true);
        NavigateToUserCommand         = new RelayCommand(() => PageContent.Content = new UserBoard        (EBoardMode.Search, ""), () => true);
        NavigateToOfferCommand        = new RelayCommand(() => PageContent.Content = new OfferBoard       (EBoardMode.Search, ""), () => true);
        NavigateToInterventionCommand = new RelayCommand(() => PageContent.Content = new InterventionBoard(EBoardMode.Search, ""), () => true);

        DataContext = this;
    }
}