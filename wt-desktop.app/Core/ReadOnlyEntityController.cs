using System;
using Avalonia.Controls;
using Avalonia.Layout;
using SukiUI.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef;
using wt_desktop.ef.Entity;

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

public abstract class ReadOnlyEntityController<E> where E : WtEntity, new()
{
    protected Window? MainWindow { get; }

    protected ReadOnlyEntityController()
    {
        MainWindow = Avalonia.Application.Current!.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : throw new Exception("MainWindow is null");
    }

    public abstract UserControl GetBoard(EBoardMode mode, string search);

    public virtual UserControl ShowBoard(EBoardMode mode, string search)
    {
        return GetBoard(mode, search);
    }
}