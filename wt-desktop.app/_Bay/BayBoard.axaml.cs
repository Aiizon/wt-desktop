using Avalonia.Controls;
using Avalonia.Interactivity;
using wt_desktop.app.Core.Controls;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Core;

public partial class BayBoard : UserControl
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