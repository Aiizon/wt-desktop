using wt_desktop.app.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Admin;

public partial class OfferBoard : BaseBoard
{
    public OfferBoard
    (
        OfferController controller,
        EBoardMode      mode,
        string          search
    ) {
        InitializeComponent();

        DataContext = new OfferBoardManager(controller, search);
    }

    public OfferBoard(EBoardMode mode, string search) : this(new OfferController(), mode, search) { }
}

public class OfferBoardManager : BoardManager<Offer>
{
    #region Filters
    public bool ShowActive
    {
        get
        {
            if (Filters.TryGetValue(nameof(Offer.IsActive), out var filter))
            {
                return filter.IsEnabled;
            }
            return false;
        }
        set
        {
            if (value == Filters[nameof(Offer.IsActive)].IsEnabled)
            {
                return;
            }
            ToggleFilter(nameof(Offer.IsActive), value);
            ApplyFilters();
        }
    }
    

    #endregion

    public OfferBoardManager(OfferController controller, string search) : base(controller, search)
    {
        RegisterFilter(nameof(Offer.IsActive), (e) => e.IsActive);
    }
}