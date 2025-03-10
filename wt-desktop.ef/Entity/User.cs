using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace wt_desktop.ef.Entity;

public class User : WtEntity
{
    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    public Array Roles { get; set; }

    [Required]
    public bool isVerified { get; set; }

    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        throw new NotImplementedException();
    }
}
