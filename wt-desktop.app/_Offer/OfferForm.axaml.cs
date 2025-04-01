using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using wt_desktop.app.Controls;
using wt_desktop.app.Core;
using wt_desktop.ef.Entity;

namespace wt_desktop.app;

public partial class OfferForm : BaseForm
{
    public OfferForm
    (
        OfferController controller, 
        EFormMode       mode,
        Offer           entity
    ) {
        InitializeComponent();

        DataContext = new OfferFormManager(controller, mode, entity);
    }

    public OfferForm(EFormMode mode, Offer entity) : this(new OfferController(), mode, entity) { }
}

public class OfferFormManager : FormManager<Offer>
{
    #region Properties
    #endregion
    
    public OfferFormManager
    (
        OfferController controller, 
        EFormMode       mode, 
        Offer           entity
    ): base(controller, mode, entity) {
        Reset();
    }

    public override bool Save()
    {
        if (!Validate())
        {
            return false;
        }
        
        
        
        return true;
    }

    public sealed override void Reset()
    {
        
    }

    public override bool Cancel()
    {
        return true;
    }

    protected override void ValidateProperty(string propertyName)
    {
        ClearErrors(propertyName);

        switch (propertyName)
        {
            
        }
    }

    public override void ValidateForm()
    {
        
    }
}