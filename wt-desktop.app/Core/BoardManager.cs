using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using wt_desktop.ef;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Core;

public class BoardManager<E>: INotifyPropertyChanged, IBoardManager where E: WtEntity, new()
{
    public EntityController<E> Controller { get; }

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
    #endregion

    #region Actions
    public Action<E> AddAction    { get; set; }
    public Action<E> EditAction   { get; set; }
    public Action<E> RemoveAction { get; set; }
    #endregion

    #region Commands
    public ICommand SearchCommand  { get; }
    public ICommand AddCommand     { get; }
    public ICommand EditCommand    { get; }
    public ICommand RemoveCommand  { get; }
    #endregion

    protected BoardManager
    (
        EntityController<E> controller,
        string              searchText
    ) {
        Controller = controller;
        SearchText = searchText;

        #region Commands
        SearchCommand = new RelayCommand(ReloadSource, () => true);
        AddCommand    = new RelayCommand(Add   , () => true);
        EditCommand   = new RelayCommand(Edit  , () => SelectedEntity != null);
        RemoveCommand = new RelayCommand(Remove, () => SelectedEntity != null);
        #endregion

        ReloadSource();
    }
    
    #region Callbacks
    private void Add()
    {
        if (AddAction != null)
        {
            AddAction.Invoke(new E());
            ReloadSource();
        }
        else
        {
            Controller.AddEntity(new E(), ReloadSource);
        }
    }

    private void Edit()
    {
        if (SelectedEntity != null && EditAction != null)
        {
            EditAction.Invoke(SelectedEntity);
            ReloadSource();
        }
        else
        {
            Controller.EditEntity(SelectedEntity!, ReloadSource);
        }
    }

    private void Remove()
    {
        if (SelectedEntity != null && RemoveAction != null)
        {
            RemoveAction.Invoke(SelectedEntity);
            ReloadSource();
        }
        else
        {
            Controller.RemoveEntity(SelectedEntity!, ReloadSource);
        }
    }
    #endregion
    
    /// <summary>
    /// Recharge les entités
    /// </summary>
    public void ReloadSource()
    {
        // Récupère l'éventuelle méthode Source() de l'entité
        // L'objectif est de forcer l'inclusion de clés étrangères s'il y en a
        Type          entityType   = typeof(E);
        MethodInfo?   sourceMethod = entityType.GetMethod("Source");
        IQueryable<E> query;

        if (sourceMethod != null)
        {
            query = (IQueryable<E>)sourceMethod.Invoke(null, null)!;
        }
        else
        {
            query = WtContext.Instance.Set<E>().AsQueryable();
        }
        
        
        EntitiesSource = !string.IsNullOrWhiteSpace(SearchText)  ?
            query.ToList().Where(x => x.MatchSearch(SearchText)) :
            query.ToList();
    }

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName]string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}