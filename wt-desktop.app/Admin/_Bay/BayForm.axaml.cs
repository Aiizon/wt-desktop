using System.Text.RegularExpressions;
using wt_desktop.app.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Admin;

public partial class BayForm : BaseForm
{
    public BayForm
    (
        BayController controller, 
        EFormMode     mode,
        Bay           entity
    ) {
        InitializeComponent();

        DataContext = new BayFormManager(controller, mode, entity);
    }

    public BayForm(EFormMode mode, Bay entity) : this(new BayController(), mode, entity) { }
}

public class BayFormManager : FormManager<Bay>
{
    #region Properties
    private string? _name;
    
    public string? Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }
    
    private string? _location;
    
    public string? Location
    {
        get => _location;
        set
        {
            _location = value;
            OnPropertyChanged();
        }
    }
    #endregion
    
    public BayFormManager
    (
        BayController controller, 
        EFormMode     mode, 
        Bay           entity
    ): base(controller, mode, entity) {
        Reset();
    }

    protected override bool Save()
    {
        if (!Validate())
        {
            return false;
        }
        
        CurrentEntity!.Name    = Name     ?? "";
        CurrentEntity.Location = Location ?? "";

        return true;
    }

    protected sealed override void Reset()
    {
        Name     = CurrentEntity!.Name;
        Location = CurrentEntity.Location;
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
            case nameof(Name):
                if (string.IsNullOrWhiteSpace(Name))
                {
                    SetError(nameof(Name), "Le nom ne peut pas être vide.");
                }
                
                if (Name!.Length != 4)
                {
                    SetError(nameof(Name), "Le nom doit contenir exactement 4 caractères.");
                }
                
                if (!Regex.IsMatch(Name!, @"^[a-zA-Z0-9\s]+$"))
                {
                    SetError(nameof(Name), "Le nom ne peut contenir que des lettres, des chiffres et des espaces.");
                }
                
                if (!Regex.IsMatch(Name!, @"B[0-9]{3}"))
                {
                    SetError(nameof(Name), "Le nom doit commencer par 'B' suivi de 3 chiffres.");
                }
                break;
            
            case nameof(Location):
                if (string.IsNullOrWhiteSpace(Location))
                {
                    SetError(nameof(Location), "L'emplacement ne peut pas être vide.");
                }
                
                if (Location!.Length > 50)
                {
                    SetError(nameof(Location), "L'emplacement ne peut pas dépasser 50 caractères.");
                }
                
                if (Location.Length < 2)
                {
                    SetError(nameof(Location), "L'emplacement doit contenir au moins 2 caractères.");
                }
                
                if (!Regex.IsMatch(Location!, @"^[a-zA-Z0-9\s]+$"))
                {
                    SetError(nameof(Location), "L'emplacement ne peut contenir que des lettres, des chiffres et des espaces.");
                }
                break;
        }
    }

    protected override void ValidateForm()
    {
        ValidateProperty(nameof(Name));
        ValidateProperty(nameof(Location));
    }
}