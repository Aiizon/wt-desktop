using System.Linq;
using wt_desktop.app.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Accounting;

public partial class UserBoard : ReadOnlyBaseBoard
{
    public UserBoard
    (
        UserController controller,
        EBoardMode     mode,
        string         search
    ) {
        InitializeComponent();

        DataContext = new UserBoardManager(controller, search);
    }

    public UserBoard(EBoardMode mode, string search) : this(new UserController(), mode, search) { }
}

public class UserBoardManager : ReadOnlyBoardManager<User>
{
    public UserBoardManager(UserController controller, string search) : base(controller, search)
    {
        EntitiesSource = EntitiesSource!
            .Where(u => u.Type == "customer")
            .ToList();
    }
}