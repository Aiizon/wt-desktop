using System;
using System.Collections.Generic;
using System.Windows.Input;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Core;

public interface IFormManager
{ 
    ICommand CancelCommand { get; }
    ICommand ResetCommand  { get; }
    ICommand SaveCommand   { get; }
}