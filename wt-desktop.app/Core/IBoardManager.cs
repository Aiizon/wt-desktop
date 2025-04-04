using System;
using System.Collections.Generic;
using System.Windows.Input;
using wt_desktop.ef;

namespace wt_desktop.app.Core;

/// <summary>
/// Interface de gestion de la liste des entités en lecture seule.
/// </summary>
public interface IReadOnlyBoardManager
{
    string? SearchText          { get; set; }
    bool    HasFilters          { get; }

    ICommand SearchCommand      { get; }
    ICommand ApplyFilterCommand { get; }
    ICommand ResetFilterCommand { get; }
}

/// <summary>
/// Interface de gestion de la liste des entités en lecture / écriture.
/// </summary>
public interface IBoardManager : IReadOnlyBoardManager
{
    ICommand AddCommand    { get; }
    ICommand EditCommand   { get; }
    ICommand RemoveCommand { get; }
}