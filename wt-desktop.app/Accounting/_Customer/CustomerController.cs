using System;
using Avalonia.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Accounting;

public class CustomerController : ReadOnlyEntityController<User>
{
    public CustomerController() {}

    public override UserControl GetBoard(EBoardMode mode, string search)
    {
        return new CustomerBoard(mode, search);
    }
}