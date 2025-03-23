using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Core;

public class RolesEditorManager
{
    private User   _User;
    private string _NewRole = string.Empty;
    
    public string NewRole
    {
        get => _NewRole;
        set
        {
            _NewRole = value;
        }
    }
    
    private ObservableCollection<string> _Roles          = new();
    private ObservableCollection<string> _AvailableRoles = new(User.UserRoles);

    public ObservableCollection<string> Roles
    {
        get => _Roles;
        set
        {
            _Roles = value;
        }
    }
    public ObservableCollection<string> AvailableRoles => _AvailableRoles;
    
    public RolesEditorManager(User user)
    {
        _User = user;
        RefreshRoles();
        
        AddRoleCommand    = new RelayCommand        (AddRole   , ()     => true);
        RemoveRoleCommand = new RelayCommand<string>(RemoveRole, (role) => true);
    }
    
    public ICommand AddRoleCommand    { get; }
    public ICommand RemoveRoleCommand { get; }
    
    public void AddRole()
    {
        if (string.IsNullOrWhiteSpace(_NewRole) || _Roles.Contains(_NewRole))
        {
            return;
        }
        
        _User.AddRole(NewRole);
        RefreshRoles();
        NewRole = string.Empty;
    }
    
    public void RemoveRole(string? role)
    {
        if (role == null)
        {
            return;
        }
        
        _User.RemoveRole(role);
        RefreshRoles();
    }
    
    private void RefreshRoles()
    {
        Roles.Clear();

        if (_User?.RolesList != null)
        {
            foreach (var role in _User.RolesList)
            {
                Roles.Add(role);
            }
        }
    }
}