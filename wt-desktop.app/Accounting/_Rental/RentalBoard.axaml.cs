using wt_desktop.app.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Accounting;

public partial class RentalBoard : ReadOnlyBaseBoard
{
    public RentalBoard
    (
        RentalController controller,
        EBoardMode     mode,
        string         search
    ) {
        InitializeComponent();

        DataContext = new RentalBoardManager(controller, search);
    }

    public RentalBoard(EBoardMode mode, string search) : this(new RentalController(), mode, search) { }
}

public class RentalBoardManager : ReadOnlyBoardManager<Rental>
{
    public RentalBoardManager(RentalController controller, string search) : base(controller, search) { }
}