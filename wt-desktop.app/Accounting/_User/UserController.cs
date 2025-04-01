using System;
using Avalonia.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Accounting;

public class UserController : EntityController<User>
{
    public UserController() {}

    public override UserControl GetBoard(EBoardMode mode, string search)
    {
        return new UserBoard(mode, search);
    }

    public override UserControl GetForm(EFormMode mode, User? entity = null)
    {
        return new UserForm(mode, entity ?? new User());
    }
}