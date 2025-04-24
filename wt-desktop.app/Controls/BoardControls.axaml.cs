using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;

namespace wt_desktop.app.Controls;

public partial class BoardControls : UserControl
{
    public static readonly StyledProperty<ICommand> AddCommandProperty    =
        AvaloniaProperty.Register<BoardControls, ICommand>(nameof(AddCommand));

    public static readonly StyledProperty<ICommand> EditCommandProperty   =
        AvaloniaProperty.Register<BoardControls, ICommand>(nameof(EditCommand));
    
    public static readonly StyledProperty<ICommand> RemoveCommandProperty =
        AvaloniaProperty.Register<BoardControls, ICommand>(nameof(RemoveCommand));
    
    public ICommand AddCommand
    {
        get => GetValue(AddCommandProperty);
        set => SetValue(AddCommandProperty, value);
    }
    
    public ICommand EditCommand
    {
        get => GetValue(EditCommandProperty);
        set => SetValue(EditCommandProperty, value);
    }
    
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