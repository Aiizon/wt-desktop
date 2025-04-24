using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using CommunityToolkit.Mvvm.Input;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Controls;

public partial class RolesEditor : UserControl
{
    private RolesEditorManager? _manager;
    
    public RolesEditor()
    {
        InitializeComponent();

        DataContextChanged += OnDataContextChanged;
    }
    
    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is RolesEditorManager manager)
        {
            _manager = manager;
            _manager.Roles.CollectionChanged -= OnRolesCollectionChanged;
        }
        
        _manager = DataContext as RolesEditorManager;
        
        if (_manager != null)
        {
            _manager.Roles.CollectionChanged += OnRolesCollectionChanged;

            PopulateRoles();
        }
    }
    
    private void OnRolesCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        PopulateRoles();
    }
    
    private void PopulateRoles()
    {
        if (_manager == null)
        {
            return;
        }
        
        RolesPanel.Children.Clear();

        foreach (string role in _manager.Roles)
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
        
        button.Click += (_, _) => _manager?.RemoveRoleCommand.Execute(role);
        Grid.SetColumn(button, 1);

        grid.Children.Add(textBlock);
        grid.Children.Add(button);
        border.Child = grid;

        return border;
    }
}

public class RolesEditorManager
{
    private readonly User _user;

    public string NewRole { get; set; } = string.Empty;

    public ObservableCollection<string> Roles          { get; set; } = new();
    public ObservableCollection<string> AvailableRoles { get; }      = new(User.UserRoles);

    public RolesEditorManager(User user)
    {
        _user = user;
        RefreshRoles();
        
        AddRoleCommand    = new RelayCommand        (AddRole   , ()  => true);
        RemoveRoleCommand = new RelayCommand<string>(RemoveRole, (_) => true);
    }
    
    public ICommand AddRoleCommand    { get; }
    public ICommand RemoveRoleCommand { get; }
    
    private void AddRole()
    {
        if (string.IsNullOrWhiteSpace(NewRole) || Roles.Contains(NewRole))
        {
            return;
        }
        
        _user.AddRole(NewRole);
        RefreshRoles();
        NewRole = string.Empty;
    }
    
    private void RemoveRole(string? role)
    {
        if (role == null)
        {
            return;
        }
        
        _user.RemoveRole(role);
        RefreshRoles();
    }
    
    private void RefreshRoles()
    {
        Roles.Clear();

        if (!_user.RolesList.Any())
        {
            return;
        }
        
        foreach (var role in _user.RolesList)
        {
            Roles.Add(role);
        }
    }
}