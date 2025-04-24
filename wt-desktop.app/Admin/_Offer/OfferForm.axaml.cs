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

    private string _name;
    
    public string  Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    private int? _maxUnits;
    
    public int?  MaxUnits
    {
        get => _maxUnits;
        set
        {
            _maxUnits = value;
            OnPropertyChanged();
        }
    }
    
    private string _availability;
    
    public string  Availability
    {
        get => _availability;
        set
        {
            _availability = value;
            OnPropertyChanged();
        }
    }
    
    private double? _monthlyRentPrice;
    
    public double?  MonthlyRentPrice
    {
        get => _monthlyRentPrice;
        set
        {
            _monthlyRentPrice = value;
            OnPropertyChanged();
        }
    }
    
    private string _bandwidth;
    
    public string  Bandwidth
    {
        get => _bandwidth;
        set
        {
            _bandwidth = value;
            OnPropertyChanged();
        }
    }
    
    private bool _isActive;
    
    public bool  IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;
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

    protected override bool Save()
    {
        if (!Validate())
        {
            return false;
        }
        
        CurrentEntity!.Name            = Name;
        CurrentEntity.MaxUnits         = MaxUnits ?? 1;
        CurrentEntity.MonthlyRentPrice = MonthlyRentPrice ?? 0.01;
        CurrentEntity.IsActive         = IsActive;

        if (Availability.Contains("%"))
        {
            CurrentEntity.Availability = Availability;
        }
        else
        {
            CurrentEntity.Availability = Availability + " %";
        }
        
        if (Bandwidth.Contains("Mbps"))
        {
            CurrentEntity.Bandwidth = Bandwidth;
        }
        else
        {
            CurrentEntity.Bandwidth = Bandwidth + " Mbps";
        }
        
        return true;
    }

    protected sealed override void Reset()
    {
        Name             = CurrentEntity!.Name;
        MaxUnits         = CurrentEntity.MaxUnits ?? 1;
        MonthlyRentPrice = CurrentEntity.MonthlyRentPrice ?? 0.01;
        IsActive         = CurrentEntity.IsActive;
        Availability     = CurrentEntity.Availability;
        Bandwidth        = CurrentEntity.Bandwidth;
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
                    break;
                }
                
                if (Name.Length < 3)
                {
                    SetError(nameof(Name), "Le nom doit contenir au moins 3 caractères.");
                }
                
                if (Name.Length > 50)
                {
                    SetError(nameof(Name), "Le nom doit contenir au maximum 50 caractères.");
                }
                break;
            
            case nameof(MaxUnits):
                if (MaxUnits == null)
                {
                    SetError(nameof(MaxUnits), "Le nombre d'unités ne peut pas être vide.");
                    break;
                }
                
                if (MaxUnits > 42)
                {
                    SetError(nameof(MaxUnits), "Le nombre d'unités ne peut pas dépasser 42.");
                }
                break;
            
            case nameof(Availability):
                if (string.IsNullOrWhiteSpace(Availability))
                {
                    SetError(nameof(Availability), "La disponibilité ne peut pas être vide.");
                    break;
                }

                if (double.TryParse(Availability.Trim()                 , out var availabilityResult) || 
                    double.TryParse(Availability.Replace("%", "").Trim(), out availabilityResult))
                {
                    if (availabilityResult < 0)
                    {
                        SetError(nameof(Availability), "La disponibilité doit être un nombre positif.");
                    }
                    
                    if (availabilityResult > 100)
                    {
                        SetError(nameof(Availability), "La disponibilité ne peut pas dépasser 100%.");
                    }
                }
                else
                {
                    SetError(nameof(Availability), "La disponibilité doit être un nombre ou pourcentage valide.");
                }
                break;
            
            case nameof(Bandwidth):
                if (string.IsNullOrWhiteSpace(Bandwidth))
                {
                    SetError(nameof(Bandwidth), "La bande passante ne peut pas être vide.");
                    break;
                }

                if (int.TryParse(Bandwidth                           , out var bandwidthResult) ||
                    int.TryParse(Bandwidth.Replace("Mbps", "").Trim(), out bandwidthResult))
                {
                    if (bandwidthResult < 0)
                    {
                        SetError(nameof(Bandwidth), "La bande passante doit être un nombre positif.");
                    }
                }
                else
                {
                    SetError(nameof(Bandwidth), "La bande passante doit être un nombre ou une valeur en Mbps valide.");
                }
                break;
        }
    }

    protected override void ValidateForm()
    {
        ValidateProperty(nameof(Name));
        ValidateProperty(nameof(MaxUnits));
        ValidateProperty(nameof(Availability));
        ValidateProperty(nameof(Bandwidth));
    }
}