using Avalonia.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Accounting;

public class RentalController : ReadOnlyEntityController<Rental>
{
    public RentalController() {}

    protected override UserControl GetBoard(EBoardMode mode, string search)
    {
        return new RentalBoard(mode, search);
    }
}