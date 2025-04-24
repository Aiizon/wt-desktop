using Avalonia.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Admin;

public class InterventionController: EntityController<Intervention>
{
    protected override UserControl GetBoard(EBoardMode mode, string search)
    {
        return new InterventionBoard(mode, search);
    }
    
    protected override UserControl GetForm(EFormMode mode, Intervention? entity = null)
    {
        return new InterventionForm(mode, entity ?? new Intervention());
    }
}