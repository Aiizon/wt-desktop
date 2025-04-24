using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using wt_desktop.app.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Admin;

public partial class UserForm : BaseForm
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

    private string _email;
    
    public string  Email
    {
        get => _email;
        set
        {
            if (value == _email) return;
            _email = value;
            OnPropertyChanged();
        }
    }
    
    private string _firstName;
    
    public string  FirstName
    {
        get => _firstName;
        set
        {
            if (value == _firstName) return;
            _firstName = value;
            OnPropertyChanged();
        }
    }
    
    private string _lastName;
    
    public string  LastName
    {
        get => _lastName;
        set
        {
            if (value == _lastName) return;
            _lastName = value;
            OnPropertyChanged();
        }
    }
    
    private string _password;
    
    public string  Password
    {
        get => _password;
        set
        {
            if (value == _password) return;
            _password = value;
            OnPropertyChanged();
        }
    }
    
    public bool   IsPasswordEnabled => Mode == EFormMode.Create;
    public string PasswordWatermark => IsPasswordEnabled ? "Mot de passe" : "••••••••";
    
    private string _roles;
    
    public string  Roles
    {
        get => _roles;
        set
        {
            if (value == _roles) return;
            _roles = value;
            OnPropertyChanged();
        }
    }
    
    public RolesEditorManager RolesEditorManager { get; }

    private string _selectedType;
    
    public string SelectedType
    {
        get => _selectedType;
        set
        {
            _selectedType = value;
            OnPropertyChanged();
        }
    }
    
    private ObservableCollection<string> _availableTypes = new();
    
    public ObservableCollection<string>  AvailableTypes
    {
        get => _availableTypes;
        set
        {
            _availableTypes = value;
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
        AvailableTypes     = new(User.UserTypes);
        
        RolesEditorManager = new RolesEditorManager(entity);
        
        Reset();
    }

    protected override bool Save()
    {
        if (!Validate())
        {
            return false;
        }
        
        CurrentEntity!.Email    = _email;
        CurrentEntity.FirstName = _firstName;
        CurrentEntity.LastName  = _lastName;
        CurrentEntity.Type      = _selectedType;
        CurrentEntity.RolesList = RolesEditorManager.Roles.ToList();

        if (Mode == EFormMode.Create)
        {
            CurrentEntity.Password = AuthProvider.Instance.HashPassword(_password);
        }
        
        return true;
    }

    protected sealed override void Reset()
    {
        Email                    = CurrentEntity!.Email;
        FirstName                = CurrentEntity.FirstName ?? "";
        LastName                 = CurrentEntity.LastName  ?? "";
        Password                 = "";
        SelectedType             = CurrentEntity.Type;
        RolesEditorManager.Roles = new(CurrentEntity.RolesList);
    }

    protected override bool Cancel()
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
                    break;
                }
                
                if (!Regex.IsMatch(Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$"))
                {
                    SetError(nameof(Email), "L'email n'est pas valide.");
                }
                
                if (WtContext.Instance.User.Any(u => u.Email == Email))
                {
                    SetError(nameof(Email), "L'email est déjà utilisé.");
                }
                break;
            
            case nameof(FirstName):
                if (SelectedType == "user")
                {
                    break;
                }
                
                if (string.IsNullOrWhiteSpace(FirstName))
                {
                    SetError(nameof(FirstName), "Le prénom est obligatoire.");
                    break;
                }
                
                if (FirstName.Length < 2)
                {
                    SetError(nameof(FirstName), "Le prénom doit contenir au moins 2 caractères.");
                }
                
                if (FirstName.Length > 50)
                {
                    SetError(nameof(FirstName), "Le prénom ne peut pas dépasser 50 caractères.");
                }
                break;
            
            case nameof(LastName):
                if (SelectedType == "user")
                {
                    break;
                }
                
                if (string.IsNullOrWhiteSpace(LastName))
                {
                    SetError(nameof(LastName), "Le nom est obligatoire.");
                    break;
                }
                
                if (LastName.Length < 2)
                {
                    SetError(nameof(LastName), "Le nom doit contenir au moins 2 caractères.");
                }
                
                if (LastName.Length > 50)
                {
                    SetError(nameof(LastName), "Le nom ne peut pas dépasser 50 caractères.");
                }
                break;
            
            case nameof(Password):
                if (Mode != EFormMode.Create)
                {
                    break;
                }

                if (string.IsNullOrWhiteSpace(Password))
                {
                    SetError(nameof(Password), "Le mot de passe est obligatoire.");
                    break;
                }

                if (Password.Length < 8)
                {
                    SetError(nameof(Password), "Le mot de passe doit contenir au moins 8 caractères.");
                }

                if (!Password.Any(char.IsDigit))
                {
                    SetError(nameof(Password), "Le mot de passe doit contenir au moins un chiffre.");
                }
                
                if (!Password.Any(char.IsLetter))
                {
                    SetError(nameof(Password), "Le mot de passe doit contenir au moins une lettre.");
                }

                if (!Password.Any(char.IsSymbol) && !Password.Any(char.IsPunctuation))
                {
                    SetError(nameof(Password), "Le mot de passe doit contenir au moins un caractère spécial.");
                }
                
                break;
        }
    }

    protected override void ValidateForm()
    {
        ValidateProperty(nameof(Email));
        ValidateProperty(nameof(FirstName));
        ValidateProperty(nameof(LastName));
    }
}