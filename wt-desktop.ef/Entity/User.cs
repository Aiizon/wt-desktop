using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace wt_desktop.ef.Entity;

[Table("user")]
[Index(nameof(Email), IsUnique = true)]
public class User : WtIdentityEntity
{
    // @todo: filtre sur boards
    // @todo: email grisé à l'édition
    // @todo: saisie password à la création
    // @todo: BI
    // @todo: quantité unités à la création d'une baie
    // @todo: lien user <-> intervention autocomplété avec l'utilisateur connecté
    // @todo: consultation (form) en read-only pour la compta
    // @todo: quantité d'unités d'une location
    public static readonly List<string> UserRoles = new() {"ROLE_ADMIN", "ROLE_ACCOUNTANT", "ROLE_USER"};
    public static readonly List<string> UserTypes = new() {"user", "customer"};

    [Required, EmailAddress]
    [Column("email")]
    public string Email { get; set; }

    [Required]
    [Column("roles", TypeName = "json")]
    public string Roles { get; set; } = "[]";
    
    [NotMapped]
    public IEnumerable<string> RolesList
    {
        get => JsonSerializer.Deserialize<List<string>>(Roles) ?? new List<string>();
        set => Roles = JsonSerializer.Serialize(value);
    }
    
    [NotMapped]
    public string RolesString
    {
        get => string.Join(", ", RolesList).ToLower().Replace("role_", "");
    }

    [Required]
    [Column("type")]
    public string Type { get; set; }

    [Column("first_name")]
    public string? FirstName { get; set; }

    [Column("last_name")]
    public string? LastName { get; set; }

    [Column("address")]
    public string? Address { get; set; }

    [Required]
    [Column("password")]
    public string Password { get; set; }

    [Required]
    [Column("is_verified")]
    public bool isVerified { get; set; }

    [Column("picture_path")]
    public string? PicturePath { get; set; }

    public override string DisplayText => $"{FirstName ?? "Anonyme"} {LastName ?? "Anonyme"} {Email}";

    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        return;
    }
    
    public bool HasRole(string role)
    {
        return RolesList.Contains(role);
    }
    
    public void AddRole(string role)
    {
        var roles = RolesList.ToList();
        if (!roles.Contains(role))
        {
            roles.Add(role);
            RolesList = roles;
        }
    }
    
    public void RemoveRole(string role)
    {
        var roles = RolesList.ToList();
        if (roles.Contains(role))
        {
            roles.Remove(role);
            RolesList = roles;
        }
    }
}
