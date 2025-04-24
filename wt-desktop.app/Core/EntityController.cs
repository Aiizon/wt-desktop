using System;
using Avalonia.Controls;
using Avalonia.Layout;
using SukiUI.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef;
using wt_desktop.ef;

namespace wt_desktop.app.Core;

/// <summary>
/// Classe de gestion des entités.
/// </summary>
/// <typeparam name="E">Type de l'entité</typeparam>
public abstract class EntityController<E> : ReadOnlyEntityController<E> where E : WtEntity, new()
{
    /// <summary>
    /// Récupère le formulaire de l'entité.
    /// </summary>
    /// <param name="mode">Mode</param>
    /// <param name="entity">Entité</param>
    /// <returns>formulaire</returns>
    public abstract UserControl GetForm(EFormMode mode, E? entity = null);

    #region View
    public virtual E AddEntity(E entity, Action? onCompleted = null)
    {
        var form        = GetForm(EFormMode.Create, entity);
        var formManager = form.DataContext as FormManager<E> ?? throw new Exception("DataContext is not a FormManager<E>");

        var window = new SukiWindow
        {
            Title = "Création d'une entité",
            Content = form
        };

        formManager.OnCancel = () => window.Close();
        formManager.OnSave   = () =>
        {
            onCompleted?.Invoke();
            window.Close();
        };

        window.ShowDialog(MainWindow!);

        return formManager.CurrentEntity ?? throw new Exception("CurrentEntity is null");
    }

    public virtual E EditEntity(E entity, Action? onCompleted = null)
    {
        var form        = GetForm(EFormMode.Update, entity);
        var formManager = form.DataContext as FormManager<E> ?? throw new Exception("DataContext is not a FormManager<E>");

        var window = new SukiWindow
        {
            Title = "Modification d'une entité",
            Content = form
        };

        formManager.OnCancel = () => window.Close();
        formManager.OnSave   = () =>
        {
            onCompleted?.Invoke();
            window.Close();
        };

        window.ShowDialog(MainWindow!);

        return formManager.CurrentEntity ?? throw new Exception("CurrentEntity is null");
    }

    public virtual void RemoveEntity(E entity, Action? onCompleted = null)
    {
        var dialogManager = new DeletionConfirmationDialogManager("Voulez-vous vraiment supprimer cette entité ?");
        var dialog        = new DeletionConfirmationDialog(dialogManager);

        dialogManager.ConfirmAction = () =>
        {
            if (DeleteEntity(entity))
            {
                onCompleted?.Invoke();
                dialog.Close();
            }
        };

        dialogManager.CancelAction = () => dialog.Close();

        dialog.ShowDialog(MainWindow!);
    }
    #endregion

    #region Model
    public virtual bool InsertEntity(E entity)
    {
        try
        {
            WtContext.Instance.Add(entity);

            return WtContext.Instance.SaveChanges() > 0;
        }
        catch (Exception e)
        {
            throw new Exception($"Erreur lors de l'insertion de l'entité. : {e.InnerException!.Message}", e);
        }
    }

    public virtual bool UpdateEntity(E entity)
    {
        try
        {
            WtContext.Instance.Update(entity);

            return WtContext.Instance.SaveChanges() > 0;
        }
        catch (Exception e)
        {
            throw new Exception($"Erreur lors de la mise à jour de l'entité. : {e.InnerException!.Message}", e);
        }
    }

    public virtual bool DeleteEntity(E entity)
    {
        try
        {
            WtContext.Instance.Remove(entity);

            return WtContext.Instance.SaveChanges() > 0;
        }
        catch (Exception e)
        {
            throw new Exception($"Erreur lors de la suppression de l'entité. : {e.InnerException!.Message}", e);
        }
    }
    #endregion
}