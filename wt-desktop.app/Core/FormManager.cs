using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using wt_desktop.ef;

namespace wt_desktop.app.Core;

/// <summary>
/// Classe de gestion des formulaires.
/// </summary>
/// <typeparam name="E">Type de l'entité</typeparam>
public abstract class FormManager<E>: INotifyPropertyChanged, INotifyDataErrorInfo, IFormManager where E: WtEntity, new()
{
    #region Properties
    protected EntityController<E> Controller { get; }
    protected EFormMode           Mode       { get; }

    private E? _currentEntity;
    public E? CurrentEntity
    {
        get => _currentEntity;
        set
        {
            _currentEntity = value;
            OnPropertyChanged();
        }
    }
    
    private readonly Dictionary<string, List<string>> _errors = new();

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

        SaveCommand   = new RelayCommand(HandleSave, () => true);
        ResetCommand  = new RelayCommand(Reset     , () => true);
        CancelCommand = new RelayCommand(() =>
        {
            if (Cancel())
            {
                CurrentEntity = null;
                OnCancel?.Invoke();
            }
        }, () => true);
    }
    
    /// <summary>
    /// Gère l'enregistrement de l'entité
    /// </summary>
    protected virtual void HandleSave()
    {
        if (!Save())
        {
            return;
        }
        
        if (Mode == EFormMode.Create)
        {
            if (Controller.InsertEntity(CurrentEntity!))
            {
                OnSave.Invoke();
            }
        }
        else
        if (Mode == EFormMode.Update)
        {
            if (Controller.UpdateEntity(CurrentEntity!))
            {
                OnSave.Invoke();
            }
        }
    }

    /// <summary>
    /// Enregistre l'entité
    /// </summary>
    /// <returns>True si l'enregistrement s'est effectué</returns>
    protected abstract bool Save();
    
    /// <summary>
    /// Réinitialise le formulaire
    /// </summary>
    protected abstract void Reset();
    
    /// <summary>
    /// Annule l'opération
    /// </summary>
    /// <returns>true</returns>
    protected virtual bool  Cancel() => true;

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName]string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
    
    #region INotifyDataErrorInfo

    public bool HasErrors => _errors.Any();
    
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    
    /// <summary>
    /// Renvoie les erreurs pour une propriété donnée
    /// </summary>
    /// <param name="propertyName">Propriété</param>
    /// <returns></returns>
    public IEnumerable GetErrors(string? propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyName) || !_errors.TryGetValue(propertyName, out var errors))
        {
            return Array.Empty<string>();
        }
        
        return errors;
    }
    #endregion

    #region Errors
    /// <summary>
    /// Ajoute une erreur pour une propriété donnée
    /// </summary>
    /// <param name="propertyName">Propriété</param>
    /// <param name="error">Texte de l'erreur</param>
    protected void SetError(string propertyName, string error)
    {
        if (!_errors.ContainsKey(propertyName))
            _errors[propertyName] = new List<string>();
            
        if (!_errors[propertyName].Contains(error))
        {
            _errors[propertyName].Add(error);
            OnErrorsChanged(propertyName);
        }
    }
    
    /// <summary>
    /// Supprime toutes les erreurs pour une propriété donnée
    /// </summary>
    /// <param name="propertyName">Propriété</param>
    protected void ClearErrors(string propertyName)
    {
        if (_errors.ContainsKey(propertyName))
        {
            _errors.Remove(propertyName);
            OnErrorsChanged(propertyName);
        }
    }
    
    /// <summary>
    /// Supprime toutes les erreurs
    /// </summary>
    private void ClearAllErrors()
    {
        var propertyNames = _errors.Keys.ToList();
        _errors.Clear();
        
        foreach (var propertyName in propertyNames)
        {
            OnErrorsChanged(propertyName);
        }
    }
    #endregion
    
    protected virtual void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        OnPropertyChanged(nameof(HasErrors));
    }

    #region Validation
    /// <summary>
    /// Valide le formulaire
    /// </summary>
    protected abstract void ValidateForm();

    /// <summary>
    /// Actualise les erreurs
    /// </summary>
    /// <returns>True s'il y a au moins une erreur, False sinon</returns>
    protected bool Validate()
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
    #endregion
}
