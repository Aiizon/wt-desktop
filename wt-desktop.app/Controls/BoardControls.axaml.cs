using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace wt_desktop.app.Controls;

public partial class BoardControls : UserControl
{
    public static readonly StyledProperty<ICommand> AddCommandProperty    =
        AvaloniaProperty.Register<SearchButton, ICommand>(nameof(AddCommand));

    public static readonly StyledProperty<ICommand> EditCommandProperty   =
        AvaloniaProperty.Register<SearchButton, ICommand>(nameof(EditCommand));
    
    public static readonly StyledProperty<ICommand> RemoveCommandProperty =
        AvaloniaProperty.Register<SearchButton, ICommand>(nameof(RemoveCommand));
    
    [Required]
    public ICommand AddCommand
    {
        get => GetValue(AddCommandProperty);
        set => SetValue(AddCommandProperty, value);
    }
    
    [Required]
    public ICommand EditCommand
    {
        get => GetValue(EditCommandProperty);
        set => SetValue(EditCommandProperty, value);
    }
    
    [Required]
    public ICommand RemoveCommand
    {
        get => GetValue(RemoveCommandProperty);
        set => SetValue(RemoveCommandProperty, value);
    }

    public BoardControls()
    {
        InitializeComponent();
    }
}