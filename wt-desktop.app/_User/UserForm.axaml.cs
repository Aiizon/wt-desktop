using System.Linq;
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
    
    public RolesEditorManager RolesEditorManager { get; }
    #endregion
    
    public UserFormManager
    (
        UserController controller, 
        EFormMode      mode, 
        User           entity
    ): base(controller, mode, entity) {
        _User = entity;
        RolesEditorManager = new RolesEditorManager(_User);
        
        Reset();
    }

    public override bool Save()
    {
        if (!Validate())
        {
            return false;
        }
        
        CurrentEntity.Email     = Email     ?? "";
        CurrentEntity.FirstName = FirstName ?? "";
        CurrentEntity.LastName  = LastName  ?? "";
        CurrentEntity.Type      = Type      ?? "";
        CurrentEntity.RolesList = RolesEditorManager.Roles.ToList();
        
        return true;
    }

    public sealed override void Reset()
    {
        Email                    = CurrentEntity.Email;
        FirstName                = CurrentEntity.FirstName;
        LastName                 = CurrentEntity.LastName;
        Type                     = CurrentEntity.Type;
        RolesEditorManager.Roles = new(CurrentEntity.RolesList);
    }

    public override bool Cancel()
    {
        return true;
    }

    protected override void ValidateProperty(string propertyName)
    {
        ClearErrors(propertyName);

        switch (propertyName)
        {
            case nameof(_Email):
                if (string.IsNullOrWhiteSpace(_Email))
                {
                    SetError(propertyName, "L'email est obligatoire.");
                }
                break;
            
            case nameof(_FirstName):
                if (string.IsNullOrWhiteSpace(_FirstName))
                {
                    SetError(propertyName, "Le prénom est obligatoire.");
                }
                break;
            
            case nameof(_LastName):
                if (string.IsNullOrWhiteSpace(_LastName))
                {
                    SetError(propertyName, "Le nom est obligatoire.");
                }
                break;
            
            case nameof(_Type):
                // @todo: combobox for type to make sure it's a valid value
                if (string.IsNullOrWhiteSpace(_Type))
                {
                    SetError(propertyName, "Le type est obligatoire.");
                }
                break;
        }
    }

    public override void ValidateForm()
    {
        ValidateProperty(nameof(_Email));
        ValidateProperty(nameof(_FirstName));
        ValidateProperty(nameof(_LastName));
        ValidateProperty(nameof(_Type));
    }
}