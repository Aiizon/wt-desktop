using System.Linq;
using wt_desktop.ef;
using wt_desktop.ef.Entity;

namespace wt_desktop.app;

public class BayModel: WtModel<Bay>
{
    public override void ReloadSource()
    {
        EntitiesSource = !string.IsNullOrWhiteSpace(SearchText) ?
            WtContext.Instance.Bay.Where(x => x.Name.Contains(SearchText)).ToList() :
            WtContext.Instance.Bay.ToList();
    }

    public override void SaveEntity()
    {
        if (SelectedEntity == null)
        {
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
}