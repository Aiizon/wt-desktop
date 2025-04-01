using System;
using System.Collections.Generic;
using System.Windows.Input;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Core;

public interface IBoardManager
{ 
    string? SearchText { get; set; }
    
    ICommand SearchCommand { get; }
    ICommand AddCommand { get; }
    ICommand EditCommand { get; }
    ICommand RemoveCommand { get; }
}