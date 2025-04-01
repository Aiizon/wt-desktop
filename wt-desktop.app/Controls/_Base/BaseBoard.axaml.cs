using Avalonia;
using Avalonia.Controls;

namespace wt_desktop.app.Controls;

public abstract partial class BaseBoard : UserControl
{
    public static readonly StyledProperty<object> BoardContentProperty =
        AvaloniaProperty.Register<BaseBoard, object>(nameof(BoardContent));
    
    public object BoardContent
    {
        get => GetValue(BoardContentProperty);
        set => SetValue(BoardContentProperty, value);
    }
    
    public BaseBoard()
    {
        InitializeComponent();
    }
}