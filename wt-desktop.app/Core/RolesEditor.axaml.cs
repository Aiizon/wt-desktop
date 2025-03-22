using Avalonia;
using Avalonia.Controls;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Core;

public partial class RolesEditor : UserControl
{
    public static readonly AttachedProperty<User> UserProperty =
        AvaloniaProperty.RegisterAttached<RolesEditor, User>(nameof(User), typeof(RolesEditor));
    
    public User User
    {
        get => GetValue(UserProperty);
        set => SetValue(UserProperty, value);
    }
    
    public RolesEditor()
    {
        InitializeComponent();

        DataContext = new RolesEditorManager(User);
    }
}