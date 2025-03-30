using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using wt_desktop.app.Core;
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
    
    public string  Email
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
    
    public string  FirstName
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
    
    public string  LastName
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
    
    public string  Roles
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
    
    public string  Type
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

    private string _SelectedType;
    
    public string SelectedType
    {
        get => _SelectedType;
        set
        {
            _SelectedType = value;
            OnPropertyChanged();
        }
    }
    
    private ObservableCollection<string> _AvailableTypes = new();
    
    public ObservableCollection<string>  AvailableTypes
    {
        get => _AvailableTypes;
        set
        {
            _AvailableTypes = value;
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
        _User = entity;
        
        AvailableTypes = new(User.UserTypes);
        
        RolesEditorManager = new RolesEditorManager(_User);
        
        Reset();
    }

    public override bool Save()
    {
        if (!Validate())
        {
            return false;
        }
        
        CurrentEntity.Email     = Email        ?? "";
        CurrentEntity.FirstName = FirstName    ?? "";
        CurrentEntity.LastName  = LastName     ?? "";
        CurrentEntity.Type      = SelectedType ?? "";
        CurrentEntity.RolesList = RolesEditorManager.Roles.ToList();
        
        return true;
    }

    public sealed override void Reset()
    {
        Email                    = CurrentEntity.Email;
        FirstName                = CurrentEntity.FirstName;
        LastName                 = CurrentEntity.LastName;
        SelectedType             = CurrentEntity.Type;
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
            case nameof(Email):
                if (string.IsNullOrWhiteSpace(Email))
                {
                    SetError(nameof(Email), "L'email est obligatoire.");
                }
                break;
            
            // case nameof(FirstName):
            //     if (string.IsNullOrWhiteSpace(FirstName))
            //     {
            //         SetError(nameof(FirstName), "Le prénom est obligatoire.");
            //     }
            //     break;
            //
            // case nameof(LastName):
            //     if (string.IsNullOrWhiteSpace(LastName))
            //     {
            //         SetError(nameof(LastName), "Le nom est obligatoire.");
            //     }
            //     break;
        }
    }

    public override void ValidateForm()
    {
        ValidateProperty(nameof(Email));
        // ValidateProperty(nameof(FirstName));
        // ValidateProperty(nameof(LastName));
    }
}