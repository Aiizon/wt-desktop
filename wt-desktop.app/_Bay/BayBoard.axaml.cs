using wt_desktop.app.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef.Entity;

namespace wt_desktop.app;

public partial class BayBoard : BaseBoard
{
    public BayBoard
    (
        BayController controller,
        EBoardMode    mode,
        string        search
    ) {
        InitializeComponent();

        DataContext = new BayBoardManager(controller, search);
    }

    public BayBoard(EBoardMode mode, string search) : this(new BayController(), mode, search) { }
}

public class BayBoardManager : BoardManager<Bay>
{
    public BayBoardManager(BayController controller, string search) : base(controller, search) { }
}