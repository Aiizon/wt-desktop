using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using wt_desktop.ef;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Core;

public abstract class ReadOnlyBoardManager<E>: INotifyPropertyChanged where E: WtEntity, new()
{
    public virtual ReadOnlyEntityController<E> Controller { get; }

    #region Properties
    private IEnumerable<E>? _EntitiesSource = null;
    public IEnumerable<E>? EntitiesSource
    {
        get => _EntitiesSource;
        set
        {
            _EntitiesSource = value;
            OnPropertyChanged();
        }
    }
    
    private ObservableCollection<E> _EntitiesSourceFiltered = new();
    
    public ObservableCollection<E>  EntitiesSourceFiltered => _EntitiesSourceFiltered;

    private E? _SelectedEntity = null;
    public E? SelectedEntity
    {
        get => _SelectedEntity;
        set
        {
            _SelectedEntity = value;
            OnPropertyChanged();
        }
    }

    private string? _SearchText = null;
    public string? SearchText
    {
        get => _SearchText;
        set
        {
            _SearchText = value;
            OnPropertyChanged();
        }
    }
    
    protected Dictionary<string, (bool IsEnabled, Func<E, bool> Predicate)> _Filters = new();
    #endregion

    #region Commands
    public ICommand SearchCommand      { get; }
    public ICommand ResetFilterCommand { get; }
    #endregion

    protected ReadOnlyBoardManager(
        ReadOnlyEntityController<E> controller,
        string searchText
    ) {
        Controller = controller;
        SearchText = searchText;

        SearchCommand      = new RelayCommand(ReloadSource, () => true);
        ResetFilterCommand = new RelayCommand(ResetFilters, () => true);

        ReloadSource();
    }

    /// <summary>
    /// Recharge les entités
    /// </summary>
    public void ReloadSource()
    {
        // Récupère l'éventuelle méthode Source() de l'entité
        // L'objectif est de forcer l'inclusion de clés étrangères s'il y en a
        Type entityType = typeof(E);
        MethodInfo? sourceMethod = entityType.GetMethod("Source");
        IQueryable<E> query;

        if (sourceMethod != null)
        {
            query = (IQueryable<E>)sourceMethod.Invoke(null, null)!;
        }
        else
        {
            query = WtContext.Instance.Set<E>().AsQueryable();
        }

        EntitiesSource = !string.IsNullOrWhiteSpace(SearchText) ?
            query.ToList().Where(x => x.MatchSearch(SearchText)) :
            query.ToList();
    }

    protected void ApplyFilters()
    {
        _EntitiesSourceFiltered.Clear();

        if (EntitiesSource == null)
        {
            return;
        }
        
        var filtered = EntitiesSource.Where(entity =>
            _Filters.Values.All(filter => !filter.IsEnabled || filter.Predicate(entity))
        );
        
        _EntitiesSourceFiltered = new(filtered);
        OnPropertyChanged(nameof(EntitiesSourceFiltered));
    }
    
    protected void ToggleFilter(string key, bool isEnabled)
    {
        if (_Filters.ContainsKey(key))
        {
            var (_, predicate) = _Filters[key];
            _Filters[key] = (isEnabled, predicate);
            ApplyFilters();
        }

        ReloadSource();
    }

    public void ResetFilters()
    {
        foreach (var key in _Filters.Keys.ToList())
        {
            var (_, predicate) = _Filters[key];
            _Filters[key] = (false, predicate);
        }
        
        ApplyFilters();
        UpdateFilterProperties();
    }

    protected virtual void UpdateFilterProperties() { }

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName]string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}