using Avalonia;
using Avalonia.Controls;

namespace wt_desktop.app.Controls;

public abstract partial class BaseForm : UserControl
{
    public static readonly StyledProperty<object> FormContentProperty =
        AvaloniaProperty.Register<BaseBoard, object>(nameof(FormContent));
    
    public object FormContent
    {
        get => GetValue(FormContentProperty);
        set => SetValue(FormContentProperty, value);
    }
    
    protected BaseForm()
    {
        InitializeComponent();
    }
}