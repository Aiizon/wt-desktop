using Avalonia.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Admin;

public class UnitController : EntityController<Unit>
{
    public UnitController() {}

    protected override UserControl GetBoard(EBoardMode mode, string search)
    {
        return new UnitBoard(mode, search);
    }

    protected override UserControl GetForm(EFormMode mode, Unit? entity = null)
    {
        return new UnitForm(mode, entity ?? new Unit());
    }
}