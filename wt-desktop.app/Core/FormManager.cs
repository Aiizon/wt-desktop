using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Core;

public abstract class FormManager<E>: INotifyPropertyChanged where E: WtEntity, new()
{
    public EntityController<E> Controller { get; }
    public EFormMode           Mode       { get; }

    private E? _CurrentEntity = null;
    public E? CurrentEntity
    {
        get { return _CurrentEntity; }
        set
        {
            _CurrentEntity = value;
            OnPropertyChanged();
        }
    }

    public Action OnSave   { get; set; }
    public Action OnCancel { get; set; }

    public ICommand SaveCommand   { get; }
    public ICommand ResetCommand  { get; }
    public ICommand CancelCommand { get; }

    protected FormManager
    (
        EntityController<E> controller,
        EFormMode mode,
        E entity
    ) {
        Controller    = controller;
        Mode          = mode;
        CurrentEntity = entity;

        SaveCommand   = new RelayCommand(() =>
        {
            if (Save())
            {
                if (Mode == EFormMode.Create)
                {
                    if (Controller.InsertEntity(CurrentEntity))
                        OnSave?.Invoke();
                }
                else
                if (Mode == EFormMode.Update)
                {
                    if (Controller.UpdateEntity(CurrentEntity))
                        OnSave?.Invoke();
                }
            }
        });
        ResetCommand  = new RelayCommand(Reset);
        CancelCommand = new RelayCommand(() =>
        {
            if (Cancel())
            {
                CurrentEntity = null;
                OnCancel?.Invoke();
            }
        });
    }

    public abstract bool Save();
    public abstract void Reset();
    public virtual bool  Cancel() => true;

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName]string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}