using Avalonia.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Admin;

public class OfferController : EntityController<Offer>
{
    public OfferController() {}

    public override UserControl GetBoard(EBoardMode mode, string search)
    {
        return new OfferBoard(mode, search);
    }

    public override UserControl GetForm(EFormMode mode, Offer? entity = null)
    {
        return new OfferForm(mode, entity ?? new Offer());
    }
}