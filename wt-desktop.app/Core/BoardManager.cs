using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using wt_desktop.ef;

namespace wt_desktop.app.Core;

/// <summary>
/// Classe de gestion de la liste des entités.
/// </summary>
/// <typeparam name="E">Type de l'entité</typeparam>
public abstract class BoardManager<E>: ReadOnlyBoardManager<E>, IBoardManager where E: WtEntity, new()
{
    #region Properties
    protected override EntityController<E> Controller => (EntityController<E>)base.Controller;

    private E? _selectedEntity;
    public E? SelectedEntity
    {
        get => _selectedEntity;
        set
        {
            _selectedEntity = value;
            ((RelayCommand)EditCommand)  .NotifyCanExecuteChanged();
            ((RelayCommand)RemoveCommand).NotifyCanExecuteChanged();
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