using System.Windows.Input;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;

namespace wt_desktop.app.Module;

public partial class AdminModule : UserControl
{
    public ICommand NavigateToBayCommand { get; }

    public AdminModule()
    {
        InitializeComponent();

        NavigateToBayCommand = new RelayCommand(() => PageContent.Content = new BayBoard(EBoardMode.Search, ""), () => true);

        DataContext = this;
    }
}