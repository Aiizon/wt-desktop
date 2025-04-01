using System;
using Avalonia.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Accounting;

public class UserController : ReadOnlyEntityController<User>
{
    public UserController() {}

    public override UserControl GetBoard(EBoardMode mode, string search)
    {
        return new UserBoard(mode, search);
    }
}