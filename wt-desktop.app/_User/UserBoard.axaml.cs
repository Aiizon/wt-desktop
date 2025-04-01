using Avalonia.Controls;
using wt_desktop.app.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef.Entity;

namespace wt_desktop.app;

public partial class UserBoard : BaseBoard
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

public class UserBoardManager : BoardManager<User>
{
    public UserBoardManager(UserController controller, string search) : base(controller, search) { }
}