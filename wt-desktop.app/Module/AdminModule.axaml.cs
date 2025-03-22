using System.Windows.Input;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using wt_desktop.app.Core;

namespace wt_desktop.app.Module;

public partial class AdminModule : UserControl
{
    public ICommand NavigateToBayCommand { get; } 
    public ICommand NavigateToUserCommand { get; } 

    public AdminModule()
    {
        InitializeComponent();

        NavigateToBayCommand  = new RelayCommand(() => PageContent.Content = new BayBoard(EBoardMode.Search, ""), () => true);
        NavigateToUserCommand = new RelayCommand(() => PageContent.Content = new UserBoard(EBoardMode.Search, ""), () => true);

        DataContext = this;
    }
}