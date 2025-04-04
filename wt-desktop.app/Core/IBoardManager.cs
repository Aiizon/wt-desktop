using System.Windows.Input;

namespace wt_desktop.app.Core;

/// <summary>
/// Interface de gestion de la liste des entités en lecture seule.
/// </summary>
public interface IReadOnlyBoardManager
{
    string? SearchText     { get; set; }
    
    ICommand SearchCommand { get; }
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