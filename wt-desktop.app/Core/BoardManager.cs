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

public class BoardManager<E>: INotifyPropertyChanged where E: WtEntity, new()
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
    public Action<E> ChooseAction { get; set; }
    #endregion

    #region Commands
    public ICommand SearchCommand  { get; }
    public ICommand AddCommand     { get; }
    public ICommand EditCommand    { get; }
    public ICommand RemoveCommand  { get; }
    public ICommand ChooseCommand  { get; }
    #endregion

    [SuppressMessage("ReSharper.DPA", "DPA0007: Large number of DB records", MessageId = "count: 1260")]
    protected BoardManager
    (
        EntityController<E> controller,
        string              searchText
    ) {
        Controller = controller;
        SearchText = searchText;

        #region Commands
        SearchCommand = new RelayCommand(ReloadSource, () => true);
        AddCommand    = new RelayCommand(
            () =>
            {
                if (AddAction != null)
                {
                    AddAction.Invoke(new E());
                }
                else
                {
                    controller.AddEntity(new E());
                }

                ReloadSource();
            },
            () => true);

        EditCommand   = new RelayCommand(
            () =>
            {
                if (SelectedEntity != null && EditAction != null)
                {
                    EditAction.Invoke(SelectedEntity);
                }
                else
                {
                    controller.EditEntity(SelectedEntity);
                }

                ReloadSource();
            },
            () => SelectedEntity != null);

        RemoveCommand = new RelayCommand(
            () =>
            {
                if (SelectedEntity != null && RemoveAction != null)
                {
                    RemoveAction.Invoke(SelectedEntity);
                }
                else
                {
                    controller.RemoveEntity(SelectedEntity);
                }

                ReloadSource();
            },
            () => SelectedEntity != null);

        ChooseCommand = new RelayCommand(
            () =>
            {
                if (SelectedEntity != null && ChooseAction != null)
                {
                    ChooseAction.Invoke(SelectedEntity);
                }
            },
            () => SelectedEntity != null);
        #endregion

        ReloadSource();
    }

    public void ReloadSource()
    {
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