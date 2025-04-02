using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace wt_desktop.app.Controls;

public partial class FormControls : UserControl
{
    public static readonly StyledProperty<ICommand> CancelCommandProperty    =
        AvaloniaProperty.Register<SearchButton, ICommand>(nameof(CancelCommand));

    public static readonly StyledProperty<ICommand> ResetCommandProperty   =
        AvaloniaProperty.Register<SearchButton, ICommand>(nameof(ResetCommand));
    
    public static readonly StyledProperty<ICommand> SaveCommandProperty =
        AvaloniaProperty.Register<SearchButton, ICommand>(nameof(SaveCommand));
    
    [Required]
    public ICommand CancelCommand
    {
        get => GetValue(CancelCommandProperty);
        set => SetValue(CancelCommandProperty, value);
    }
    
    [Required]
    public ICommand ResetCommand
    {
        get => GetValue(ResetCommandProperty);
        set => SetValue(ResetCommandProperty, value);
    }
    
    [Required]
    public ICommand SaveCommand
    {
        get => GetValue(SaveCommandProperty);
        set => SetValue(SaveCommandProperty, value);
    }

    public FormControls()
    {
        InitializeComponent();
    }
}