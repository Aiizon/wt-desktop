using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;

namespace wt_desktop.app.Controls;

public partial class AddButton : UserControl
{
    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<SearchButton, ICommand>(nameof(Command));

    public ICommand Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public AddButton()
    {
        InitializeComponent();
    }
}