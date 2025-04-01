using Avalonia.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Admin;

public class UnitController : EntityController<Unit>
{
    public UnitController() {}

    public override UserControl GetBoard(EBoardMode mode, string search)
    {
        return new UnitBoard(mode, search);
    }

    public override UserControl GetForm(EFormMode mode, Unit? entity = null)
    {
        return new UnitForm(mode, entity ?? new Unit());
    }
}