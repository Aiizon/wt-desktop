using System.Windows.Input;

namespace wt_desktop.app.Core;

/// <summary>
/// Interface de gestion des formulaires.
/// </summary>
public interface IFormManager
{ 
    ICommand CancelCommand { get; }
    ICommand ResetCommand  { get; }
    ICommand SaveCommand   { get; }
}