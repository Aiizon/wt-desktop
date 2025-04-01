using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using wt_desktop.app.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Admin;

public partial class UnitBoard : BaseBoard
{
    public UnitBoard
    (
        UnitController controller,
        EBoardMode     mode,
        string         search
    ) {
        InitializeComponent();

        DataContext = new UnitBoardManager(controller, search);
    }

    public UnitBoard(EBoardMode mode, string search) : this(new UnitController(), mode, search) { }
}

public class UnitBoardManager : BoardManager<Unit>
{
    public UnitBoardManager(UnitController controller, string search) : base(controller, search) { }
}