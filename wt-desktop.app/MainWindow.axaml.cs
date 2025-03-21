using System.Windows.Input;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.Input;
using SukiUI.Controls;
using wt_desktop.app.Controls;
using wt_desktop.app.Module;

namespace wt_desktop.app;

public partial class MainWindow : SukiWindow
{
    public ICommand AdminModuleCommand      { get; }
    public ICommand AccountantModuleCommand { get; }
    
    public MainWindow()
    {
        InitializeComponent();

        AdminModuleCommand      = new RelayCommand(() => MainContent.Content = new AdminModule(), () => true);
        AccountantModuleCommand = new RelayCommand(() => MainContent.Content = null, () => true);
        
        DataContext = this;
    }
}