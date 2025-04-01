using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SukiUI.Controls;
using wt_desktop.app.Core;
using wt_desktop.app.Module;
using wt_desktop.ef.Entity;

namespace wt_desktop.app;

public partial class MainWindow : SukiWindow
{
    public User CurrentUser => AuthProvider.Instance.CurrentUser!;
    
    public bool IsAdmin => CurrentUser.HasRole("ROLE_ADMIN");
    
    public ICommand AdminModuleCommand      { get; }
    public ICommand AccountantModuleCommand { get; }
    
    public MainWindow()
    {
        InitializeComponent();

        AdminModuleCommand      = new RelayCommand(() => MainContent.Content = new AdminModule()     , () => true);
        AccountantModuleCommand = new RelayCommand(() => MainContent.Content = new AccountingModule(), () => true);
        

        if (CurrentUser.HasRole("ROLE_ADMIN"))
        {
            AdminModuleCommand.Execute(null);
        }
        else if (CurrentUser.HasRole("ROLE_ACCOUNTANT"))
        {
            AccountantModuleCommand.Execute(null);
        }
        
        DataContext = this;
    }
}