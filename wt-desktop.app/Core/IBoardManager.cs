using System.Windows.Input;

namespace wt_desktop.app.Core;

public interface IReadOnlyBoardManager
{
    string? SearchText     { get; set; }
    
    ICommand SearchCommand { get; }
}

public interface IBoardManager : IReadOnlyBoardManager
{
    ICommand AddCommand    { get; }
    ICommand EditCommand   { get; }
    ICommand RemoveCommand { get; }
}