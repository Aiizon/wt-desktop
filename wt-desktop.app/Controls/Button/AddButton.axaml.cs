using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace wt_desktop.app.Controls;

public partial class SaveButton : UserControl
{
    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<SearchButton, ICommand>(nameof(Command));

    public ICommand Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
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
                Command.Execute(null);
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