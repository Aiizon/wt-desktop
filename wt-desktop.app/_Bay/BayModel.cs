using System.Linq;
using wt_desktop.ef;
using wt_desktop.ef.Entity;

namespace wt_desktop.app;

public class BayModel: WtModel<Bay>
{
    public override void AddEntity()
    {
        if (SelectedEntity == null)
        {
            // WtContext.Instance.Bay.Add(new Bay());
            return;
        }

        if (SelectedEntity.Id == 0)
        {
            WtContext.Instance.Add(SelectedEntity);
        }
        else
        {
            WtContext.Instance.Update(SelectedEntity);
        }

        WtContext.Instance.SaveChanges();
        ReloadSource();
    }

    public override void EditEntity()
    {
        if (SelectedEntity == null)
        {
            return;
        }

        Bay? entity = WtContext.Instance.Bay.Find(SelectedEntity.Id);
        if (entity == null)
        {
            return;
        }

        SelectedEntity = entity;
    }

    public override void RemoveEntity()
    {
        if (SelectedEntity == null)
        {
            return;
        }

        WtContext.Instance.Remove(SelectedEntity);
        WtContext.Instance.SaveChanges();
        ReloadSource();
    }

    public override void SaveChanges()
    {
        WtContext.Instance.SaveChanges();
        ReloadSource();
    }
}