using Avalonia.Controls;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Core;

public class BayController : EntityController<Bay>
{
    public BayController() {}

    public override UserControl GetBoard(EBoardMode mode, string search)
    {
        return new BayBoard(mode, search);
    }

    public override UserControl GetForm(EFormMode mode, Bay? entity = null)
    {
        return new BayForm(mode, entity ?? new Bay());
    }
}