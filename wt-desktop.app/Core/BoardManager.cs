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

public abstract class BoardManager<E>: ReadOnlyBoardManager<E>, IBoardManager where E: WtEntity, new()
{
    public override EntityController<E> Controller => (EntityController<E>)base.Controller;
    
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
    ): base(controller, searchText)
    {
        #region Commands
        AddCommand    = new RelayCommand(Add   , () => true);
        EditCommand   = new RelayCommand(Edit  , () => SelectedEntity != null);
        RemoveCommand = new RelayCommand(Remove, () => SelectedEntity != null);
        #endregion
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
}