using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;

namespace wt_desktop.app.Controls;

public partial class SearchBox : UserControl
{
    public static readonly StyledProperty<string>   SearchTextProperty    =
        AvaloniaProperty.Register<SearchBox, string>  (nameof(SearchText));

    public static readonly StyledProperty<ICommand> SearchCommandProperty =
        AvaloniaProperty.Register<SearchBox, ICommand>(nameof(SearchCommand));
    
    public string SearchText
    {
        get => GetValue(SearchTextProperty);
        set => SetValue(SearchTextProperty, value);
    }
    
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