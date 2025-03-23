using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace wt_desktop.app.Core;

public partial class RolesEditor : UserControl
{
    private RolesEditorManager? _Manager;
    
    public RolesEditor()
    {
        InitializeComponent();

        DataContextChanged += OnDataContextChanged;
    }
    
    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is RolesEditorManager manager)
        {
            _Manager = manager;
            _Manager.Roles.CollectionChanged -= OnRolesCollectionChanged;
        }
        
        _Manager = DataContext as RolesEditorManager;
        
        if (_Manager != null)
        {
            _Manager.Roles.CollectionChanged += OnRolesCollectionChanged;

            PopulateRoles();
        }
    }
    
    private void OnRolesCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        PopulateRoles();
    }
    
    private void PopulateRoles()
    {
        if (_Manager == null)
        {
            return;
        }
        
        RolesPanel.Children.Clear();

        foreach (string role in _Manager.Roles)
        {
            RolesPanel.Children.Add(CreateRoleElement(role));
        }
    }
    
    private Control CreateRoleElement(string role)
    {
        var border = new Border
        {
            Background   = new SolidColorBrush(Color.Parse("#1E90FF")),
            CornerRadius = new CornerRadius(4),
            Margin       = new Thickness(0, 0, 5, 5),
            Padding      = new Thickness(5, 2)
        };

        var grid = new Grid();
        grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));

        var textBlock = new TextBlock
        {
            Text              = role,
            Foreground        = Brushes.White,
            VerticalAlignment = VerticalAlignment.Center
        };
        Grid.SetColumn(textBlock, 0);

        var button = new Button
        {
            Content = "✕",
            Padding = new Thickness(2),
            Margin  = new Thickness(5, 0, 0, 0)
        };
        
        button.Click += (s, e) => _Manager?.RemoveRoleCommand.Execute(role);
        Grid.SetColumn(button, 1);

        grid.Children.Add(textBlock);
        grid.Children.Add(button);
        border.Child = grid;

        return border;
    }
}