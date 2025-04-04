using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using wt_desktop.app.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Admin;

public partial class UnitForm : BaseForm
{
    public UnitForm
    (
        UnitController controller, 
        EFormMode      mode,
        Unit           entity
    ) {
        InitializeComponent();

        DataContext = new UnitFormManager(controller, mode, entity);
    }

    public UnitForm(EFormMode mode, Unit entity) : this(new UnitController(), mode, entity) { }
}

public class UnitFormManager : FormManager<Unit>
{
    #region Properties
    private string? _Name;
    
    public string?  Name
    {
        get => _Name;
        set
        {
            _Name = value;
            OnPropertyChanged();
        }
    }
    
    private Bay? _SelectedBay;
    
    public Bay?  SelectedBay
    {
        get => _SelectedBay;
        set
        {
            _SelectedBay = value;
            OnPropertyChanged();
        }
    }
    
    private ObservableCollection<Bay> _AvailableBays = new();
    
    public ObservableCollection<Bay>  AvailableBays
    {
        get => _AvailableBays;
        set
        {
            _AvailableBays = value;
            OnPropertyChanged();
        }
    }
    #endregion
    
    public UnitFormManager
    (
        UnitController controller, 
        EFormMode      mode, 
        Unit           entity
    ): base(controller, mode, entity)
    {
        AvailableBays = new(WtContext.Instance.Set<Bay>().ToList());
        
        Reset();
    }

    public override bool Save()
    {
        if (!Validate())
        {
            return false;
        }
        
        CurrentEntity.Name = Name ?? "";
        CurrentEntity.Bay  = AvailableBays.FirstOrDefault(b => b.Id == SelectedBay?.Id);
        
        return true;
    }

    public sealed override void Reset()
    {
        Name        = CurrentEntity.Name;
        SelectedBay = AvailableBays.FirstOrDefault(b => b.Id == CurrentEntity.Bay?.Id);
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
                    break;
                }
                
                if (Name!.Length != 4)
                {
                    SetError(nameof(Name), "Le nom doit contenir exactement 4 caractères.");
                }
                
                if (!Regex.IsMatch(Name!, @"^[a-zA-Z0-9\s]+$"))
                {
                    SetError(nameof(Name), "Le nom ne peut contenir que des lettres, des chiffres et des espaces.");
                }
                
                if (!Regex.IsMatch(Name!, @"U[0-9]{3}"))
                {
                    SetError(nameof(Name), "Le nom doit commencer par 'U' suivi de 3 chiffres.");
                }
                break;
            
            case nameof(SelectedBay):
                if 
                (
                    SelectedBay    == null ||
                    SelectedBay.Id == 0    ||
                    WtContext.Instance.Bay.Find(SelectedBay.Id) == null
                ) 
                {
                    SetError(nameof(SelectedBay), "La baie est obligatoire.");
                }
                break;
        }
    }

    public override void ValidateForm()
    {
        ValidateProperty(nameof(Name));
        ValidateProperty(nameof(SelectedBay));
    }
}