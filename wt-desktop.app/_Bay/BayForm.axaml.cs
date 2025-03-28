using Avalonia.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef.Entity;

namespace wt_desktop.app;

public partial class BayForm : UserControl
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
    private string? _Name;
    
    public string? Name
    {
        get => _Name;
        set
        {
            _Name = value;
            OnPropertyChanged();
        }
    }
    
    private string? _Location;
    
    public string? Location
    {
        get => _Location;
        set
        {
            _Location = value;
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

    public override bool Save()
    {
        if (!Validate())
        {
            return false;
        }
        
        CurrentEntity.Name     = Name     ?? "";
        CurrentEntity.Location = Location ?? "";

        return true;
    }

    public sealed override void Reset()
    {
        Name     = CurrentEntity.Name;
        Location = CurrentEntity.Location;
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
            case nameof(Name):
                if (string.IsNullOrWhiteSpace(Name))
                {
                    SetError(nameof(Name), "Le nom ne peut pas être vide.");
                }
                break;
            
            case nameof(Location):
                if (string.IsNullOrWhiteSpace(Location))
                {
                    SetError(nameof(Location), "L'emplacement ne peut pas être vide.");
                }
                break;
        }
    }

    public override void ValidateForm()
    {
        ValidateProperty(nameof(Name));
        ValidateProperty(nameof(Location));
    }
}