using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef;
using wt_desktop.ef.Entity;

namespace wt_desktop.app;

public partial class UnitForm : UserControl
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
    
    public string? Name
    {
        get => _Name;
        set
        {
            _Name = value;
            OnPropertyChanged();
        }
    }
    
    private Bay? _SelectedBay;
    
    public Bay? SelectedBay
    {
        get => _SelectedBay;
        set
        {
            _SelectedBay = value;
            OnPropertyChanged();
        }
    }
    
    private ObservableCollection<Bay> _AvailableBays = new();
    
    public ObservableCollection<Bay> AvailableBays
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
        CurrentEntity.Name = Name ?? "";
        CurrentEntity.Bay  = AvailableBays.FirstOrDefault(b => b.Id == SelectedBay?.Id) ?? new Bay();
        
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
}