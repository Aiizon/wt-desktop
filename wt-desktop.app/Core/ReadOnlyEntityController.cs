using System;
using Avalonia.Controls;
using wt_desktop.ef;

namespace wt_desktop.app.Core;

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

/// <summary>
/// Classe de gestion des entités en lecture seule.
/// </summary>
/// <typeparam name="E">Type de l'entité</typeparam>
public abstract class ReadOnlyEntityController<E> where E : WtEntity, new()
{
    protected Window? MainWindow { get; }

    protected ReadOnlyEntityController()
    {
        MainWindow = Avalonia.Application.Current!.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : throw new Exception("MainWindow is null");
    }

    /// <summary>
    /// Récupère le tableau de l'entité.
    /// </summary>
    /// <param name="mode">Mode</param>
    /// <param name="search">Chaîne de recherche</param>
    /// <returns>tableau</returns>
    protected abstract UserControl GetBoard(EBoardMode mode, string search);

    /// <summary>
    /// Affiche le tableau de l'entité.
    /// </summary>
    /// <param name="mode">Mode</param>
    /// <param name="search">Chaîne de recherche</param>
    /// <returns>tableau</returns>
    public virtual UserControl ShowBoard(EBoardMode mode, string search)
    {
        return GetBoard(mode, search);
    }
}