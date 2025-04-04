using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
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
    private string? _Comment;
    
    public string? Comment
    {
        get => _Comment;
        set
        {
            _Comment = value;
            OnPropertyChanged();
        }
    }
    
    private DateTimeOffset? _StartDate;
    
    public DateTimeOffset?  StartDate
    {
        get => _StartDate;
        set
        {
            _StartDate = value;
            OnPropertyChanged();
        }
    }
    
    private DateTimeOffset? _EndDate;
    
    public DateTimeOffset?  EndDate
    {
        get => _EndDate;
        set
        {
            _EndDate = value;
            OnPropertyChanged();
        }
    }
    
    private ObservableCollection<Bay?>  _AvailableBays  = new();
    private ObservableCollection<Unit?> _AvailableUnits = new();
    
    public ObservableCollection<Bay?>   AvailableBays
    {
        get => _AvailableBays;
        set => _AvailableBays = value;
    }

    public ObservableCollection<Unit?>  AvailableUnits
    {
        get => _AvailableUnits;
        set => _AvailableUnits = value;
    }
    
    private ObservableCollection<Bay?>  _SelectedBays  = new();
    
    public ObservableCollection<Bay?>   SelectedBays
    {
        get => _SelectedBays;
        set
        {
            _SelectedBays = value;
            OnPropertyChanged();
        }
    }
    
    private ObservableCollection<Unit?> _SelectedUnits = new();
    
    public ObservableCollection<Unit?>  SelectedUnits
    {
        get => _SelectedUnits;
        set
        {
            _SelectedUnits = value;
            OnPropertyChanged();
        }
    }

    private string _UnitSearchText;
    
    public string UnitSearchText
    {
        get => _UnitSearchText;
        set
        {
            _UnitSearchText = value;
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
        
        _AvailableBays  = new(WtContext.Instance.Bay.ToList()!);
        _AvailableUnits = _SelectedUnits;
    }
    
    public override bool Save()
    {
        if (!Validate())
        {
            return false;
        }
        
        CurrentEntity.Comment   = Comment ?? "";
        CurrentEntity.StartDate = StartDate!.Value.DateTime;
        CurrentEntity.EndDate   = EndDate?.DateTime;
        
        return true;
    }

    public override void HandleSave()
    {
        if (!Save())
        {
            return;
        }
        
        if (Mode == EFormMode.Create)
        {
            if (Controller.InsertEntity(CurrentEntity))
            {
                InterventionUnitHandler.HandleSave(CurrentEntity, SelectedBays, SelectedUnits);
                OnSave?.Invoke();
            }
        }
        else
        if (Mode == EFormMode.Update)
        {
            // @fixme: update crashes on second save (entity is already tracked)
            if (Controller.UpdateEntity(CurrentEntity))
            {
                InterventionUnitHandler.HandleSave(CurrentEntity, SelectedBays, SelectedUnits);
                OnSave?.Invoke();
            }
        }
    }

    public override void Reset()
    {
        Comment   = CurrentEntity.Comment;
        StartDate = new DateTimeOffset(CurrentEntity.StartDate ?? DateTime.Now);
        EndDate   = new DateTimeOffset(CurrentEntity.EndDate   ?? DateTime.Now);
        
        (_SelectedBays, _SelectedUnits) = InterventionUnitHandler.HandleReset(CurrentEntity);
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
    
    public override void ValidateForm()
    {
        ValidateProperty(nameof(Comment));
        ValidateProperty(nameof(StartDate));
        ValidateProperty(nameof(EndDate));
    }

    private void SearchUnits()
    {
        if (UnitSearchText.Length < 3)
        {
            _AvailableUnits = new(_SelectedUnits);
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
        
        _AvailableUnits.Clear();
        
        foreach (var unit in queryResult)
        {
            if (!unitsInSelectedBays.Contains(unit.Id))
            {
                _AvailableUnits.Add(unit);
            }
        }
        
        OnPropertyChanged(nameof(AvailableUnits));
    }
}