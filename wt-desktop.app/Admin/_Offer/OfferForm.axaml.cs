using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using wt_desktop.app.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Admin;

public partial class OfferForm : BaseForm
{
    public OfferForm
    (
        OfferController controller, 
        EFormMode       mode,
        Offer           entity
    ) {
        InitializeComponent();

        DataContext = new OfferFormManager(controller, mode, entity);
    }

    public OfferForm(EFormMode mode, Offer entity) : this(new OfferController(), mode, entity) { }
}

public class OfferFormManager : FormManager<Offer>
{
    #region Properties

    private string _Name;
    
    public string Name
    {
        get => _Name;
        set
        {
            _Name = value;
            OnPropertyChanged();
        }
    }

    private int _MaxUnits;
    
    public int MaxUnits
    {
        get => _MaxUnits;
        set
        {
            _MaxUnits = value;
            OnPropertyChanged();
        }
    }
    
    private string _Availability;
    
    public string Availability
    {
        get => _Availability;
        set
        {
            _Availability = value;
            OnPropertyChanged();
        }
    }
    
    private double _MonthlyRentPrice;
    
    public double MonthlyRentPrice
    {
        get => _MonthlyRentPrice;
        set
        {
            _MonthlyRentPrice = value;
            OnPropertyChanged();
        }
    }
    
    private string _Bandwidth;
    
    public string Bandwidth
    {
        get => _Bandwidth;
        set
        {
            _Bandwidth = value;
            OnPropertyChanged();
        }
    }
    
    private bool _IsActive;
    
    public bool IsActive
    {
        get => _IsActive;
        set
        {
            _IsActive = value;
            OnPropertyChanged();
        }
    }
    #endregion
    
    public OfferFormManager
    (
        OfferController controller, 
        EFormMode       mode, 
        Offer           entity
    ): base(controller, mode, entity) {
        Reset();
    }

    public override bool Save()
    {
        if (!Validate())
        {
            return false;
        }
        
        CurrentEntity.Name             = Name;
        CurrentEntity.MaxUnits         = MaxUnits;
        CurrentEntity.Availability     = Availability;
        CurrentEntity.MonthlyRentPrice = MonthlyRentPrice;
        CurrentEntity.Bandwidth        = Bandwidth;
        CurrentEntity.IsActive         = IsActive;
        
        return true;
    }

    public sealed override void Reset()
    {
        Name             = CurrentEntity.Name;
        MaxUnits         = CurrentEntity.MaxUnits;
        Availability     = CurrentEntity.Availability;
        MonthlyRentPrice = CurrentEntity.MonthlyRentPrice;
        Bandwidth        = CurrentEntity.Bandwidth;
        IsActive         = CurrentEntity.IsActive;
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
                    SetError(propertyName, "Le nom ne peut pas être vide.");
                }
                break;
            case nameof(MaxUnits):
                if (MaxUnits <= 0)
                {
                    SetError(propertyName, "Le nombre d'unités doit être supérieur à 0.");
                }
                break;
            case nameof(MonthlyRentPrice):
                if (MonthlyRentPrice <= 0)
                {
                    SetError(propertyName, "Le prix doit être supérieur à 0.");
                }
                break;
            case nameof(Availability):
                if (string.IsNullOrWhiteSpace(Availability))
                {
                    SetError(propertyName, "La disponibilité ne peut pas être vide.");
                }
                break;
        }
    }

    public override void ValidateForm()
    {
        ValidateProperty(nameof(Name));
        ValidateProperty(nameof(MaxUnits));
        ValidateProperty(nameof(MonthlyRentPrice));
        ValidateProperty(nameof(Availability));
    }
}