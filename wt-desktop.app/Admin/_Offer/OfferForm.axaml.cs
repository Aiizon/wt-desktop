using System.Collections.ObjectModel;
using System.Globalization;
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
    
    public string  Name
    {
        get => _Name;
        set
        {
            _Name = value;
            OnPropertyChanged();
        }
    }

    private int? _MaxUnits;
    
    public int?  MaxUnits
    {
        get => _MaxUnits;
        set
        {
            _MaxUnits = value;
            OnPropertyChanged();
        }
    }
    
    private string _Availability;
    
    public string  Availability
    {
        get => _Availability;
        set
        {
            _Availability = value;
            OnPropertyChanged();
        }
    }
    
    private double? _MonthlyRentPrice;
    
    public double?  MonthlyRentPrice
    {
        get => _MonthlyRentPrice;
        set
        {
            _MonthlyRentPrice = value;
            OnPropertyChanged();
        }
    }
    
    private string _Bandwidth;
    
    public string  Bandwidth
    {
        get => _Bandwidth;
        set
        {
            _Bandwidth = value;
            OnPropertyChanged();
        }
    }
    
    private bool _IsActive;
    
    public bool  IsActive
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

    public sealed override void Reset()
    {
        Name             = CurrentEntity.Name;
        MaxUnits         = CurrentEntity.MaxUnits ?? 1;
        MonthlyRentPrice = CurrentEntity.MonthlyRentPrice ?? 0.01;
        IsActive         = CurrentEntity.IsActive;
        Availability     = CurrentEntity.Availability;
        Bandwidth        = CurrentEntity.Bandwidth;
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

                double availabilityResult;
                if (double.TryParse(Availability.Trim()                 , out availabilityResult) || 
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

                int bandwidthResult;
                if (int.TryParse(Bandwidth                           , out bandwidthResult) ||
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

    public override void ValidateForm()
    {
        ValidateProperty(nameof(Name));
        ValidateProperty(nameof(MaxUnits));
        ValidateProperty(nameof(Availability));
        ValidateProperty(nameof(Bandwidth));
    }
}