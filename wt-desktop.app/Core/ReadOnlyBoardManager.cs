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

/// <summary>
/// Classe de gestion de la liste des entités en lecture seule.
/// </summary>
/// <typeparam name="E">Type de l'entité</typeparam>
public abstract class ReadOnlyBoardManager<E>: INotifyPropertyChanged, IReadOnlyBoardManager where E: WtEntity, new()
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

    private Dictionary<string, (bool IsEnabled, Func<E, bool> Predicate)> _Filters = new();
    
    public bool HasFilters => _Filters.Any();
    #endregion

    #region Commands
    public ICommand SearchCommand      { get; }
    public ICommand ApplyFilterCommand { get; }
    public ICommand ResetFilterCommand { get; }
    #endregion

    protected ReadOnlyBoardManager(
        ReadOnlyEntityController<E> controller,
        string searchText
    ) {
        Controller = controller;
        SearchText = searchText;

        SearchCommand      = new RelayCommand(ReloadSource, () => true);
        ApplyFilterCommand = new RelayCommand(ApplyFilters, () => true);
        ResetFilterCommand = new RelayCommand(ResetFilters, () => true);

        ReloadSource();
    }

    /// <summary>
    /// Recharge les entités
    /// </summary>
    public void ReloadSource()
    {
        var query = WtContext.Instance.Set<E>().AsQueryable();

        EntitiesSource = !string.IsNullOrWhiteSpace(SearchText) ?
            query.ToList().Where(x => x.MatchSearch(SearchText)).ToList() :
            query.ToList();
    }

    // WIP
    #region Filters
    /// <summary>
    /// Ajoute un filtre à la liste des entités
    /// </summary>
    /// <param name="key">Nom de la propriété</param>
    /// <param name="isEnabled">Le filtre est-il actif?</param>
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
    
    /// <summary>
    /// Ajoute un filtre à la liste des filtres
    /// </summary>
    /// <param name="key">Nom de la propriété</param>
    /// <param name="predicate">Prédicat de validité</param>
    /// <param name="isEnabled">Le filtre est-il actif par défaut?</param>
    protected void RegisterFilter(string key, Func<E, bool> predicate, bool isEnabled = false)
    {
        _Filters[key] = (isEnabled, predicate);
    }

    /// <summary>
    /// Réinitialise les filtres
    /// </summary>
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
    
    /// <summary>
    /// Applique les filtres sur la liste des entités
    /// </summary>
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

    /// <summary>
    /// Met à jour les propriétés des filtres
    /// </summary>
    protected virtual void UpdateFilterProperties() { }
    #endregion

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName]string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}