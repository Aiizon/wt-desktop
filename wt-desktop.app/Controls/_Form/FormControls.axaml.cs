using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;

namespace wt_desktop.app.Controls;

public partial class FormControls : UserControl
{
    public static readonly StyledProperty<ICommand> CancelCommandProperty    =
        AvaloniaProperty.Register<FormControls, ICommand>(nameof(CancelCommand));

    public static readonly StyledProperty<ICommand> ResetCommandProperty   =
        AvaloniaProperty.Register<FormControls, ICommand>(nameof(ResetCommand));
    
    public static readonly StyledProperty<ICommand> SaveCommandProperty =
        AvaloniaProperty.Register<FormControls, ICommand>(nameof(SaveCommand));
    
    public ICommand CancelCommand
    {
        get => GetValue(CancelCommandProperty);
        set => SetValue(CancelCommandProperty, value);
    }
    
    public ICommand ResetCommand
    {
        get => GetValue(ResetCommandProperty);
        set => SetValue(ResetCommandProperty, value);
    }
    
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