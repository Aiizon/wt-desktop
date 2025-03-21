using System;
using Avalonia.Controls;
using Avalonia.Layout;
using SukiUI.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef;
using wt_desktop.ef.Entity;

namespace wt_desktop.app;

public enum EFormMode
{
    Create,
    Update,
    Read
}

public enum EBoardMode
{
    Search,
    Read
}

public abstract class EntityController<E> where E : WtEntity, new()
{
    private Window? MainWindow { get; }

    protected EntityController()
    {
        MainWindow = Avalonia.Application.Current!.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : throw new Exception("MainWindow is null");
    }

    public abstract UserControl GetForm(EFormMode mode, E? entity = null);
    public abstract UserControl GetBoard(EBoardMode mode, string search);

    #region View
    public virtual E ChooseEntity(string search)
    {
        var board        = GetBoard(EBoardMode.Search, search);
        var boardManager = board.DataContext as BoardManager<E> ?? throw new Exception("DataContext is not a BoardManager<E>");

        var window = new SukiWindow
        {
            Title = "Selection d'une entité",
            Content = board
        };

        boardManager.ChooseAction = (e) => window.Close();

        window.ShowDialog(MainWindow!);

        return boardManager.SelectedEntity ?? throw new Exception("SelectedEntity is null");
    }

    public virtual UserControl ShowBoard(EBoardMode mode, string search)
    {
        return GetBoard(mode, search);
    }

    public virtual E AddEntity(E entity)
    {
        var form        = GetForm(EFormMode.Create, entity);
        var formManager = form.DataContext as FormManager<E> ?? throw new Exception("DataContext is not a FormManager<E>");

        var window = new SukiWindow
        {
            Title = "Création d'une entité",
            Content = form
        };

        formManager.OnSave   = () => window.Close();
        formManager.OnCancel = () => window.Close();

        window.ShowDialog(MainWindow!);

        return formManager.CurrentEntity ?? throw new Exception("CurrentEntity is null");
    }

    public virtual E EditEntity(E entity)
    {
        var form        = GetForm(EFormMode.Update, entity);
        var formManager = form.DataContext as FormManager<E> ?? throw new Exception("DataContext is not a FormManager<E>");

        var window = new SukiWindow
        {
            Title = "Modification d'une entité",
            Content = form
        };

        formManager.OnSave   = () => window.Close();
        formManager.OnCancel = () => window.Close();

        window.ShowDialog(MainWindow!);

        return formManager.CurrentEntity ?? throw new Exception("CurrentEntity is null");
    }

    public virtual void RemoveEntity(E entity)
    {
        var dialogManager = new DeletionConfirmationDialogManager("Voulez-vous vraiment supprimer cette entité ?");
        var dialog        = new DeletionConfirmationDialog(dialogManager);

        dialogManager.ConfirmAction = () =>
        {
            if (DeleteEntity(entity))
            {
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
        WtContext.Instance.Add(entity);

        return WtContext.Instance.SaveChanges() > 0;
    }

    public virtual bool UpdateEntity(E entity)
    {
        WtContext.Instance.Update(entity);

        return WtContext.Instance.SaveChanges() > 0;
    }

    public virtual bool DeleteEntity(E entity)
    {
        WtContext.Instance.Remove(entity);

        return WtContext.Instance.SaveChanges() > 0;
    }
    #endregion
}