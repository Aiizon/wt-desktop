using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;

namespace wt_desktop.app.Controls;

public partial class FilterBox : UserControl
{
    public static readonly StyledProperty<object> FilterContentProperty =
        AvaloniaProperty.Register<FilterBox, object> (nameof(FilterContent));

    public static readonly StyledProperty<ICommand> ResetFilterCommandProperty =
        AvaloniaProperty.Register<FilterBox, ICommand>(nameof(ResetFilterCommand));

    public static readonly StyledProperty<ICommand> ApplyFilterCommandProperty =
        AvaloniaProperty.Register<FilterBox, ICommand>(nameof(ApplyFilterCommand));
    
    public static readonly DirectProperty<FilterBox, bool> HasFiltersProperty =
        AvaloniaProperty.RegisterDirect<FilterBox, bool>(
            nameof(HasFilters), 
            o => o.HasFilters, 
            (o, v) => o.HasFilters = v);
    
    public object FilterContent
    {
        get => GetValue(FilterContentProperty);
        set
        {
            SetValue(FilterContentProperty, value);
            UpdateHasFilters(value);
        }
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
    
    private bool _hasFilters;
    public bool HasFilters
    {
        get => _hasFilters;
        set => SetAndRaise(HasFiltersProperty, ref _hasFilters, value);
    }

    public FilterBox()
    {
        InitializeComponent();
    }
    
    private void UpdateHasFilters(object? value)
    {
        HasFilters = value != null && !string.Equals(value.ToString(), string.Empty, StringComparison.Ordinal);
    }
}