using Avalonia.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Admin;

public class BayController : EntityController<Bay>
{
    public BayController() {}

    protected override UserControl GetBoard(EBoardMode mode, string search)
    {
        return new BayBoard(mode, search);
    }

    protected override UserControl GetForm(EFormMode mode, Bay? entity = null)
    {
        return new BayForm(mode, entity ?? new Bay());
    }
}