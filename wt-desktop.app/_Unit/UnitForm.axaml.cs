using Avalonia.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef.Entity;

namespace wt_desktop.app;

public partial class UnitForm : UserControl
{
    public UnitForm
    (
        UnitController controller, 
        EFormMode      mode,
        Unit           entity
    ) {
        InitializeComponent();

        DataContext = new UnitFormManager(controller, mode, entity);
    }

    public UnitForm(EFormMode mode, Unit entity) : this(new UnitController(), mode, entity) { }
}

public class UnitFormManager : FormManager<Unit>
{
    #region Properties
    #endregion
    
    public UnitFormManager
    (
        UnitController controller, 
        EFormMode      mode, 
        Unit           entity
    ): base(controller, mode, entity) {
        Reset();
    }

    public override bool Save()
    {

        return true;
    }

    public sealed override void Reset()
    {
    }

    public override bool Cancel()
    {
        return true;
    }
}