using System.Linq;
using wt_desktop.app.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Accounting;

public partial class CustomerBoard : ReadOnlyBaseBoard
{
    public CustomerBoard
    (
        CustomerController controller,
        EBoardMode         mode,
        string             search
    ) {
        InitializeComponent();

        DataContext = new CustomerBoardManager(controller, search);
    }

    public CustomerBoard(EBoardMode mode, string search) : this(new CustomerController(), mode, search) { }
}

public class CustomerBoardManager : ReadOnlyBoardManager<User>
{
    public CustomerBoardManager(CustomerController controller, string search) : base(controller, search)
    {
        EntitiesSource = EntitiesSource!
            .Where(u => u.Type == "customer")
            .ToList();
    }
}