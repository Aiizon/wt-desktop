using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace wt_desktop.app.Controls;

public partial class NavigationButton : UserControl
{
    public static readonly StyledProperty<string> LabelProperty =
        AvaloniaProperty.Register<ModuleButton, string>(nameof(Label));

    public static readonly StyledProperty<string> IconProperty =
        AvaloniaProperty.Register<ModuleButton, string>(nameof(Icon));

    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<ModuleButton, ICommand>(nameof(Command));

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

    protected override void OnInitialized()
    {
        base.OnInitialized();

        var button = this.FindControl<Button>("InnerButton");

        if (button != null)
        {
            button.Click += (sender, eventArgs) =>
            {
                Command.Execute(null);
                eventArgs.Handled = true;
            };
        }
    }
}