using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Core;

public abstract class FormManager<E>: INotifyPropertyChanged, INotifyDataErrorInfo, IFormManager where E: WtEntity, new()
{
    #region Properties
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
    
    private readonly Dictionary<string, List<string>> _Errors = new();

    public Action OnSave   { get; set; }
    public Action OnCancel { get; set; }

    public ICommand SaveCommand   { get; }
    public ICommand ResetCommand  { get; }
    public ICommand CancelCommand { get; }
    #endregion

    protected FormManager
    (
        EntityController<E> controller,
        EFormMode           mode,
        E                   entity
    ) {
        Controller    = controller;
        Mode          = mode;
        CurrentEntity = entity;

        SaveCommand   = new RelayCommand(HandleSave, () => !HasErrors);
        ResetCommand  = new RelayCommand(Reset, () => true);
        CancelCommand = new RelayCommand(() =>
        {
            if (Cancel())
            {
                CurrentEntity = null;
                OnCancel?.Invoke();
            }
        }, () => true);
    }
    
    public virtual void HandleSave()
    {
        if (!Save())
        {
            return;
        }
        
        if (Mode == EFormMode.Create)
        {
            if (Controller.InsertEntity(CurrentEntity))
            {
                OnSave?.Invoke();
            }
        }
        else
        if (Mode == EFormMode.Update)
        {
            if (Controller.UpdateEntity(CurrentEntity))
            {
                OnSave?.Invoke();
            }
        }
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
    
    #region INotifyDataErrorInfo

    public bool HasErrors => _Errors.Any();
    
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    
    /// <summary>
    /// Renvoie les erreurs pour une propriété donnée
    /// </summary>
    /// <param name="propertyName">Propriété</param>
    /// <returns></returns>
    public IEnumerable GetErrors(string? propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyName) || !_Errors.TryGetValue(propertyName, out var errors))
        {
            return Array.Empty<string>();
        }
        
        return errors;
    }
    #endregion
    
    // Helpers pour la gestion des erreurs
    
    /// <summary>
    /// Ajoute une erreur pour une propriété donnée
    /// </summary>
    /// <param name="propertyName">Propriété</param>
    /// <param name="error">Texte de l'erreur</param>
    protected void SetError(string propertyName, string error)
    {
        if (!_Errors.ContainsKey(propertyName))
            _Errors[propertyName] = new List<string>();
            
        if (!_Errors[propertyName].Contains(error))
        {
            _Errors[propertyName].Add(error);
            OnErrorsChanged(propertyName);
        }
    }
    
    /// <summary>
    /// Supprime toutes les erreurs pour une propriété donnée
    /// </summary>
    /// <param name="propertyName">Propriété</param>
    protected void ClearErrors(string propertyName)
    {
        if (_Errors.ContainsKey(propertyName))
        {
            _Errors.Remove(propertyName);
            OnErrorsChanged(propertyName);
        }
    }
    
    /// <summary>
    /// Supprime toutes les erreurs
    /// </summary>
    protected void ClearAllErrors()
    {
        var propertyNames = _Errors.Keys.ToList();
        _Errors.Clear();
        
        foreach (var propertyName in propertyNames)
            OnErrorsChanged(propertyName);
    }
    
    protected virtual void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        OnPropertyChanged(nameof(HasErrors));
    }
    
    // Validation du formulaire / des propriétés
    
    /// <summary>
    /// Valide le formulaire
    /// </summary>
    public abstract void ValidateForm();

    /// <summary>
    /// Actualise les erreurs
    /// </summary>
    /// <returns>True si il y a au moins une erreur, False sinon</returns>
    public bool Validate()
    {
        ClearAllErrors();
        ValidateForm();
        
        return !HasErrors;
    }
    
    /// <summary>
    /// Valide une propriété
    /// </summary>
    /// <param name="propertyName">Propriété</param>
    protected virtual void ValidateProperty(string propertyName) { }
}