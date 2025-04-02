using Avalonia;
using Avalonia.Controls;

namespace wt_desktop.app.Controls;

public abstract partial class ReadOnlyBaseBoard : UserControl
{
    public static readonly StyledProperty<object> BoardContentProperty =
        AvaloniaProperty.Register<BaseBoard, object>(nameof(BoardContent));
    
    public object BoardContent
    {
        get => GetValue(BoardContentProperty);
        set => SetValue(BoardContentProperty, value);
    }
    
    public ReadOnlyBaseBoard()
    {
        InitializeComponent();
    }
}