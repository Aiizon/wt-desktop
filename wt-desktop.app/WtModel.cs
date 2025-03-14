using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using wt_desktop.ef.Entity;
using wt_desktop.ef;

namespace wt_desktop.app;

public abstract class WtModel: INotifyPropertyChanged
{
    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName]string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}

public abstract class WtModel<TE>: WtModel where TE: WtEntity, new()
{
    private IEnumerable<TE>? _EntitiesSource = null;

    public IEnumerable<TE>? EntitiesSource
    {
        get => _EntitiesSource;
        set
        {
            _EntitiesSource = value;
            OnPropertyChanged();
        }
    }

    private TE? _SelectedEntity = null;

    public TE? SelectedEntity
    {
        get => _SelectedEntity;
        set
        {
            _SelectedEntity = value;
            OnPropertyChanged();
        }
    }

    private string? _SearchText = null;

    public string? SearchText
    {
        get => _SearchText;
        set
        {
            _SearchText = value;
            OnPropertyChanged();
        }
    }

    public virtual void ReloadSource()
    {
        EntitiesSource = WtContext.Instance.Set<TE>().ToList();
    }

    public abstract void SaveEntity();

    public abstract void EditEntity();

    public abstract void RemoveEntity();
}