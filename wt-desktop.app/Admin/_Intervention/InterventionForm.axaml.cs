using System;
using System.Collections.ObjectModel;
using System.Linq;
using wt_desktop.app.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Admin;

public partial class InterventionForm : BaseForm
{
    public InterventionForm
    (
        InterventionController controller,  
        EFormMode              mode,
        Intervention           entity
    ) {
        InitializeComponent();
        
        DataContext = new InterventionFormManager(controller, mode, entity);
    }
    
    public InterventionForm(EFormMode mode, Intervention entity) : this(new InterventionController(), mode, entity) { }
}

public class InterventionFormManager: FormManager<Intervention>
{
    #region Properties
    private string? _comment;
    
    public string? Comment
    {
        get => _comment;
        set
        {
            _comment = value;
            OnPropertyChanged();
        }
    }
    
    private DateTimeOffset? _startDate;
    
    public DateTimeOffset?  StartDate
    {
        get => _startDate;
        set
        {
            _startDate = value;
            OnPropertyChanged();
        }
    }
    
    private DateTimeOffset? _endDate;
    
    public DateTimeOffset?  EndDate
    {
        get => _endDate;
        set
        {
            _endDate = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Bay?>   AvailableBays  { get; set; }

    public ObservableCollection<Unit?>  AvailableUnits { get; set; }

    private ObservableCollection<Bay?>  _selectedBays  = new();
    
    public ObservableCollection<Bay?>   SelectedBays
    {
        get => _selectedBays;
        set
        {
            _selectedBays = value;
            OnPropertyChanged();
        }
    }
    
    private ObservableCollection<Unit?> _selectedUnits = new();
    
    public ObservableCollection<Unit?>  SelectedUnits
    {
        get => _selectedUnits;
        set
        {
            _selectedUnits = value;
            OnPropertyChanged();
        }
    }

    private string _unitSearchText;
    
    public string UnitSearchText
    {
        get => _unitSearchText;
        set
        {
            _unitSearchText = value;
            SearchUnits();
        }
    }
    #endregion
    
    public InterventionFormManager
    (
        InterventionController controller,
        EFormMode              mode,
        Intervention           entity
    ) : base(controller, mode, entity) {
        Reset();
        
        AvailableBays  = new(WtContext.Instance.Bay.ToList()!);
        AvailableUnits = _selectedUnits;
    }
    
    protected override bool Save()
    {
        if (!Validate())
        {
            return false;
        }
        
        CurrentEntity!.Comment  = Comment ?? "";
        CurrentEntity.StartDate = StartDate!.Value.DateTime;
        CurrentEntity.EndDate   = EndDate?.DateTime;
        
        return true;
    }

    protected override void HandleSave()
    {
        if (!Save())
        {
            return;
        }
        
        if (Mode == EFormMode.Create)
        {
            if (Controller.InsertEntity(CurrentEntity!))
            {
                InterventionUnitHandler.HandleSave(CurrentEntity!, SelectedBays, SelectedUnits);
                OnSave.Invoke();
            }
        }
        else
        if (Mode == EFormMode.Update)
        {
            // @fixme: update crashes on second save (entity is already tracked)
            if (Controller.UpdateEntity(CurrentEntity!))
            {
                InterventionUnitHandler.HandleSave(CurrentEntity!, SelectedBays, SelectedUnits);
                OnSave.Invoke();
            }
        }
    }

    protected sealed override void Reset()
    {
        Comment   = CurrentEntity!.Comment;
        StartDate = new DateTimeOffset(CurrentEntity.StartDate ?? DateTime.Now);
        EndDate   = new DateTimeOffset(CurrentEntity.EndDate   ?? DateTime.Now);
        
        (_selectedBays, _selectedUnits) = InterventionUnitHandler.HandleReset(CurrentEntity);
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
            case nameof(Comment):
                if (string.IsNullOrWhiteSpace(Comment))
                {
                    SetError(nameof(Comment), "Le commentaire ne peut pas être vide.");
                }
                
                if (Comment!.Length > 255)
                {
                    SetError(nameof(Comment), "Le commentaire ne peut pas dépasser 255 caractères.");
                }
                break;
            
            case nameof(StartDate):
                if (StartDate == null)
                {
                    SetError(nameof(StartDate), "La date de début ne peut pas être vide.");
                    break;
                }
                
                if (StartDate > EndDate)
                {
                    SetError(nameof(StartDate), "La date de début ne peut pas être supérieure à la date de fin.");
                }
                
                if (StartDate < DateTimeOffset.Now)
                {
                    SetError(nameof(StartDate), "La date de début ne peut pas être passée.");
                }
                break;
            
            case nameof(EndDate):
                if (EndDate == null)
                {
                    SetError(nameof(EndDate), "La date de fin ne peut pas être vide.");
                    break;
                }
                
                if (EndDate < StartDate)
                {
                    SetError(nameof(EndDate), "La date de fin ne peut pas être inférieure à la date de début.");
                }
                break;
        }
    }
    
    protected override void ValidateForm()
    {
        ValidateProperty(nameof(Comment));
        ValidateProperty(nameof(StartDate));
        ValidateProperty(nameof(EndDate));
    }

    private void SearchUnits()
    {
        if (UnitSearchText.Length < 3)
        {
            AvailableUnits = new(_selectedUnits);
            return;
        }
        
        var queryResult = WtContext.Instance.Unit
            .Where(u => u.Name.Contains(UnitSearchText) || u.Bay!.Name.Contains(UnitSearchText))
            .Take(30)
            .ToHashSet();
        
        var unitsInSelectedBays = SelectedBays
            .SelectMany(b => b!.Units())
            .Select(u => u.Id)
            .ToHashSet();
        
        AvailableUnits.Clear();
        
        foreach (var unit in queryResult)
        {
            if (!unitsInSelectedBays.Contains(unit.Id))
            {
                AvailableUnits.Add(unit);
            }
        }
        
        OnPropertyChanged(nameof(AvailableUnits));
    }
}