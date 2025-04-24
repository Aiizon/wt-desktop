using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using wt_desktop.app.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Admin;

public partial class InterventionBoard : BaseBoard
{
    public InterventionBoard
    (
        InterventionController controller,
        EBoardMode             mode,
        string                 search
    ) {
        InitializeComponent();
        
        DataContext = new InterventionBoardManager(controller, search);
    }
    
    public InterventionBoard(EBoardMode mode, string search) : this(new InterventionController(), mode, search) { }
}

public class InterventionBoardManager: BoardManager<Intervention>
{
    #region Properties
    private readonly ObservableCollection<Intervention> _currentInterventions  = new();
    private readonly ObservableCollection<Intervention> _upcomingInterventions = new();
    public  ObservableCollection<Intervention> CurrentInterventions   => _currentInterventions;
    public  ObservableCollection<Intervention> UpcomingInterventions  => _upcomingInterventions;
    
    private int _selectedIndex;
    
    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            _selectedIndex = value;
            OnPropertyChanged();
        }
    }
    #endregion

    public InterventionBoardManager(InterventionController controller, string search) : base(controller, search)
    {
        PropertyChanged += OnBoardManagerPropertyChanged;
        
        FilterInterventions();
    }

    private void OnBoardManagerPropertyChanged(object? sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == nameof(EntitiesSource))
        {
            FilterInterventions();
        }
    }
    
    private void FilterInterventions()
    {
        _currentInterventions.Clear();
        _upcomingInterventions.Clear();
        
        foreach (var intervention in EntitiesSource ?? [])
        {
            if (intervention.StartDate <= DateTime.Now && 
                intervention.EndDate   >= DateTime.Now)
            {
                _currentInterventions.Add(intervention);
            }
            else if (intervention.StartDate >= DateTime.Now)
            {
                _upcomingInterventions.Add(intervention);
            }
        }
    }
}