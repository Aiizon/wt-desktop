using Avalonia.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef.Entity;

namespace wt_desktop.app;

public class InterventionController: EntityController<Intervention>
{
    public override UserControl GetBoard(EBoardMode mode, string search)
    {
        return new InterventionBoard(mode, search);
    }
    
    public override UserControl GetForm(EFormMode mode, Intervention? entity = null)
    {
        return new InterventionForm(mode, entity ?? new Intervention());
    }
}