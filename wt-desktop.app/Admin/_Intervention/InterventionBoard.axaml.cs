using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Avalonia.Controls;
using Microsoft.EntityFrameworkCore.Query;
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
    private ObservableCollection<Intervention> _CurrentInterventions  = new();
    private ObservableCollection<Intervention> _UpcomingInterventions = new();
    public  ObservableCollection<Intervention> CurrentInterventions   => _CurrentInterventions;
    public  ObservableCollection<Intervention> UpcomingInterventions  => _UpcomingInterventions;
    
    private int _SelectedIndex = 0;
    
    public int SelectedIndex
    {
        get => _SelectedIndex;
        set
        {
            _SelectedIndex = value;
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
        _CurrentInterventions.Clear();
        _UpcomingInterventions.Clear();
        
        foreach (var intervention in EntitiesSource ?? [])
        {
            if (intervention.StartDate <= DateTime.Now && 
                intervention.EndDate   >= DateTime.Now)
            {
                _CurrentInterventions.Add(intervention);
            }
            else if (intervention.StartDate >= DateTime.Now)
            {
                _UpcomingInterventions.Add(intervention);
            }
        }
    }
}