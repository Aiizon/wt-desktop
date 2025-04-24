using Avalonia.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Admin;

public class UserController : EntityController<User>
{
    public UserController() {}

    protected override UserControl GetBoard(EBoardMode mode, string search)
    {
        return new UserBoard(mode, search);
    }

    protected override UserControl GetForm(EFormMode mode, User? entity = null)
    {
        return new UserForm(mode, entity ?? new User());
    }
}