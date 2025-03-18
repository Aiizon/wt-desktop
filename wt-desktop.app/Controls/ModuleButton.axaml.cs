using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace wt_desktop.app.Controls;

public partial class ModuleButton : UserControl
{
    public static readonly StyledProperty<string> LabelProperty =
        AvaloniaProperty.Register<ModuleButton, string>(nameof(Label));

    public static readonly StyledProperty<string> IconProperty =
        AvaloniaProperty.Register<ModuleButton, string>(nameof(Icon));

    public new static readonly RoutedEvent<RoutedEventArgs> ClickEvent =
        RoutedEvent.Register<ModuleButton, RoutedEventArgs>(nameof(Click), RoutingStrategies.Bubble);

    public new event EventHandler<RoutedEventArgs> Click
    {
        add    => AddHandler(ClickEvent, value);
        remove => RemoveHandler(ClickEvent, value);
    }

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

    public ModuleButton()
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
                RaiseEvent(new RoutedEventArgs(ClickEvent));
                eventArgs.Handled = true;
            };
        }
    }
    //
    // private void OnButtonClick(object? sender, RoutedEventArgs e)
    // {
    //     Button button = (Button)sender!;
    //     ClickEvent
    // }
}