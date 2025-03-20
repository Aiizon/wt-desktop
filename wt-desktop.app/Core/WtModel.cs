using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using wt_desktop.ef.Entity;
using wt_desktop.ef;

namespace wt_desktop.app.Core;

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

public abstract class WtModel<E>: WtModel where E: WtEntity, new()
{
    private IEnumerable<E>? _EntitiesSource = null;

    public IEnumerable<E>? EntitiesSource
    {
        get => _EntitiesSource;
        set
        {
            _EntitiesSource = value;
            OnPropertyChanged();
        }
    }

    private E? _SelectedEntity = null;

    public E? SelectedEntity
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
        EntitiesSource = !string.IsNullOrWhiteSpace(SearchText) ?
            WtContext.Instance.Set<E>().ToList().Where(x => x.MatchSearch(SearchText)) :
            WtContext.Instance.Set<E>().ToList();
    }

    public abstract void AddEntity();

    public abstract void EditEntity();

    public abstract void RemoveEntity();

    public abstract void SaveChanges();
}