using Avalonia.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef;
using wt_desktop.ef.Entity;

namespace wt_desktop.app;

public partial class UserForm : UserControl
{
    public UserForm
    (
        UserController controller, 
        EFormMode      mode,
        User           entity
    ) {
        InitializeComponent();

        DataContext = new UserFormManager(controller, mode, entity);
    }

    public UserForm(EFormMode mode, User entity) : this(new UserController(), mode, entity) { }
}

public class UserFormManager : FormManager<User>
{
    #region Properties
    private User _User;
    
    public User User
    {
        get => _User;
        set
        {
            _User = value;
            OnPropertyChanged();
        }
    }
    #endregion
    
    public UserFormManager
    (
        UserController controller, 
        EFormMode      mode, 
        User           entity
    ): base(controller, mode, entity) {
        User = entity;
        
        Reset();
    }

    public override bool Save()
    {

        return true;
    }

    public sealed override void Reset()
    {
    }

    public override bool Cancel()
    {
        return true;
    }
}