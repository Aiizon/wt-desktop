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
    private string _Email;
    
    public string Email
    {
        get => _Email;
        set
        {
            if (value == _Email) return;
            _Email = value;
            OnPropertyChanged();
        }
    }
    
    private string _FirstName;
    
    public string FirstName
    {
        get => _FirstName;
        set
        {
            if (value == _FirstName) return;
            _FirstName = value;
            OnPropertyChanged();
        }
    }
    
    private string _LastName;
    
    public string LastName
    {
        get => _LastName;
        set
        {
            if (value == _LastName) return;
            _LastName = value;
            OnPropertyChanged();
        }
    }
    
    private string _Roles;
    
    public string Roles
    {
        get => _Roles;
        set
        {
            if (value == _Roles) return;
            _Roles = value;
            OnPropertyChanged();
        }
    }
    
    private string _Type;
    
    public string Type
    {
        get => _Type;
        set
        {
            if (value == _Type) return;
            _Type = value;
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
        Reset();
    }

    public override bool Save()
    {
        CurrentEntity.Email     = Email     ?? "";
        CurrentEntity.FirstName = FirstName ?? "";
        CurrentEntity.LastName  = LastName  ?? "";
        CurrentEntity.Roles     = Roles     ?? "";
        CurrentEntity.Type      = Type      ?? "";
        
        return true;
    }

    public sealed override void Reset()
    {
        Email     = CurrentEntity.Email;
        FirstName = CurrentEntity.FirstName;
        LastName  = CurrentEntity.LastName;
        Roles     = CurrentEntity.Roles;
        Type      = CurrentEntity.Type;
    }

    public override bool Cancel()
    {
        return true;
    }
}