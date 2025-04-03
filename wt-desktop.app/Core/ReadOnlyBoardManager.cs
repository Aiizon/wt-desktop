using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using wt_desktop.ef;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Core;

public class ReadOnlyBoardManager<E>: INotifyPropertyChanged where E: WtEntity, new()
{
    public virtual ReadOnlyEntityController<E> Controller { get; }

    #region Properties
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
    #endregion

    #region Commands
    public ICommand SearchCommand { get; }
    #endregion

    protected ReadOnlyBoardManager(
        ReadOnlyEntityController<E> controller,
        string searchText
    ) {
        Controller = controller;
        SearchText = searchText;

        SearchCommand = new RelayCommand(ReloadSource, () => true);

        ReloadSource();
    }

    /// <summary>
    /// Recharge les entités
    /// </summary>
    public void ReloadSource()
    {
        var query = WtContext.Instance.Set<E>().AsQueryable();

        EntitiesSource = !string.IsNullOrWhiteSpace(SearchText) ?
            query.ToList().Where(x => x.MatchSearch(SearchText)).ToList() :
            query.ToList();
    }

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName]string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}