using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Nodes;

namespace wt_desktop.ef.Entity;

[Table("user")]
[Index(nameof(Email), IsUnique = true)]
public class User : WtEntity
{
    public enum EUserType
    {
        User,
        Customer
    }

    [Required, EmailAddress]
    [Column("email")]
    public string Email { get; set; }

    [Required]
    [Column("roles", TypeName = "json")]
    public JsonArray Roles { get; set; }

    [Required]
    [Column("type")]
    public EUserType Type { get; set; }

    [Column("first_name")]
    public string FirstName { get; set; }

    [Column("last_name")]
    public string LastName { get; set; }

    [Column("address")]
    public string Address { get; set; }

    [Required]
    [Column("password")]
    public string Password { get; set; }

    [Required]
    [Column("is_verified")]
    public bool isVerified { get; set; }

    [Column("picture_path")]
    public string PicturePath { get; set; }

    public string GetFullName()
    {
        return $"{FirstName} {LastName}";
    }

    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        return;
    }
}
