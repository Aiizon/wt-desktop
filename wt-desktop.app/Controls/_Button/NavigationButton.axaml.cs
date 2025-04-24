using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;

namespace wt_desktop.app.Controls;

public partial class NavigationButton : UserControl
{
    public static readonly StyledProperty<string> LabelProperty =
        AvaloniaProperty.Register<NavigationButton, string>(nameof(Label));

    public static readonly StyledProperty<string> IconProperty =
        AvaloniaProperty.Register<NavigationButton, string>(nameof(Icon));

    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<NavigationButton, ICommand>(nameof(Command));

    public string Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public string Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public ICommand Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public NavigationButton()
    {
        InitializeComponent();
    }
}