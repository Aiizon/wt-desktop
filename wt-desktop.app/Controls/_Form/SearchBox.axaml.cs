using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace wt_desktop.app.Controls;

public partial class SearchBox : UserControl
{
    public static readonly StyledProperty<string>   SearchTextProperty    =
        AvaloniaProperty.Register<SearchButton, string>  (nameof(SearchText));

    public static readonly StyledProperty<ICommand> SearchCommandProperty =
        AvaloniaProperty.Register<SearchButton, ICommand>(nameof(SearchCommand));
    
    public string SearchText
    {
        get => GetValue(SearchTextProperty);
        set => SetValue(SearchTextProperty, value);
    }
    
    [Required]
    public ICommand SearchCommand
    {
        get => GetValue(SearchCommandProperty);
        set => SetValue(SearchCommandProperty, value);
    }

    public SearchBox()
    {
        InitializeComponent();
    }
}