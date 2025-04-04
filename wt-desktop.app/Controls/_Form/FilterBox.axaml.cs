using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace wt_desktop.app.Controls;

public partial class FilterBox : UserControl
{
    public static readonly StyledProperty<object> FilterContentProperty =
        AvaloniaProperty.Register<SearchButton, object> (nameof(FilterContent));

    public static readonly StyledProperty<ICommand> ResetFilterCommandProperty =
        AvaloniaProperty.Register<SearchButton, ICommand>(nameof(ResetFilterCommand));

    public static readonly StyledProperty<ICommand> ApplyFilterCommandProperty =
        AvaloniaProperty.Register<SearchButton, ICommand>(nameof(ResetFilterCommand));
    
    public object FilterContent
    {
        get => GetValue(FilterContentProperty);
        set => SetValue(FilterContentProperty, value);
    }
    
    public ICommand ResetFilterCommand
    {
        get => GetValue(ResetFilterCommandProperty);
        set => SetValue(ResetFilterCommandProperty, value);
    }
    
    public ICommand ApplyFilterCommand
    {
        get => GetValue(ApplyFilterCommandProperty);
        set => SetValue(ApplyFilterCommandProperty, value);
    }

    public FilterBox()
    {
        InitializeComponent();
    }
}