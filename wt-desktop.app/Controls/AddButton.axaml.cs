using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace wt_desktop.app.Controls;

public partial class SaveButton : UserControl
{
    public new static readonly RoutedEvent<RoutedEventArgs> ClickEvent =
        RoutedEvent.Register<ModuleButton, RoutedEventArgs>(nameof(Click), RoutingStrategies.Bubble);

    public new event EventHandler<RoutedEventArgs> Click
    {
        add    => AddHandler(ClickEvent, value);
        remove => RemoveHandler(ClickEvent, value);
    }

    public SaveButton()
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