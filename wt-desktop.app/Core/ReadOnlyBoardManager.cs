using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using wt_desktop.ef;

namespace wt_desktop.app.Core;

/// <summary>
/// Classe de gestion de la liste des entités en lecture seule.
/// </summary>
/// <typeparam name="E">Type de l'entité</typeparam>
public abstract class ReadOnlyBoardManager<E>: INotifyPropertyChanged, IReadOnlyBoardManager where E: WtEntity, new()
{
    protected virtual ReadOnlyEntityController<E> Controller { get; }

    #region Properties
    private IEnumerable<E>? _entitiesSource;
    public IEnumerable<E>? EntitiesSource
    {
        get => _entitiesSource;
        set
        {
            _entitiesSource         = value;
            _entitiesSourceFiltered = new ObservableCollection<E>(value ?? new List<E>());
            OnPropertyChanged();
            OnPropertyChanged(nameof(EntitiesSourceFiltered));
        }
    }
    
    private ObservableCollection<E> _entitiesSourceFiltered = new();
    
    public ObservableCollection<E>  EntitiesSourceFiltered => _entitiesSourceFiltered;

    private string? _searchText;
    public string? SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged();
        }
    }

    protected readonly Dictionary<string, (bool IsEnabled, Func<E, bool> Predicate)> Filters = new();
    
    public bool HasFilters => Filters.Any();
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
    protected void ReloadSource()
    {
        var query = WtContext.Instance.Set<E>().AsQueryable();

        try
        {
            EntitiesSource = !string.IsNullOrWhiteSpace(SearchText) ?
                query.ToList().Where(x => x.MatchSearch(SearchText)).ToList() :
                query.ToList();
        }
        catch (Exception e)
        {
            throw new Exception("Erreur lors du rechargement des entités.", e);
        }
        
        if (HasFilters)
        {
            ApplyFilters();
        }
    }

    #region Filters
    /// <summary>
    /// Active ou désactive un filtre
    /// </summary>
    /// <param name="key">Nom de la propriété</param>
    /// <param name="isEnabled">Le filtre est-il actif ?</param>
    protected void ToggleFilter(string key, bool isEnabled)
    {
        if (Filters.ContainsKey(key))
        {
            var (_, predicate) = Filters[key];
            Filters[key] = (isEnabled, predicate);
            ApplyFilters();
        }

        ReloadSource();
    }
    
    /// <summary>
    /// Ajoute un filtre à la liste des filtres
    /// </summary>
    /// <param name="key">Nom de la propriété</param>
    /// <param name="predicate">Prédicat de validité</param>
    /// <param name="isEnabled">Le filtre est-il actif par défaut ?</param>
    protected void RegisterFilter(string key, Func<E, bool> predicate, bool isEnabled = false)
    {
        Filters[key] = (isEnabled, predicate);
    }

    /// <summary>
    /// Réinitialise les filtres
    /// </summary>
    private void ResetFilters()
    {
        foreach (var key in Filters.Keys.ToList())
        {
            var (_, predicate) = Filters[key];
            Filters[key] = (false, predicate);
            OnPropertyChanged(key);
        }
        
        ReloadSource();
        UpdateFilterProperties();
    }
    
    /// <summary>
    /// Applique les filtres sur la liste des entités
    /// </summary>
    protected void ApplyFilters()
    {
        _entitiesSourceFiltered.Clear();

        if (EntitiesSource == null)
        {
            return;
        }
        
        var filtered = EntitiesSource.Where(entity =>
            Filters.Values.All(filter => !filter.IsEnabled || filter.Predicate(entity))
        );
        
        _entitiesSourceFiltered = new(filtered);
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