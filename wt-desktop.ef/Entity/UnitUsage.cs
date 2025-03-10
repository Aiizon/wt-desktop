using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace wt_desktop.ef.Entity;

[Table("unit_usage")]
public class UnitUsage: WtEntity
{
    [Required]
    [Column("name")]
    public string Name { get; set; }

    [Required]
    [Column("color")]
    public string Color { get; set; }

    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        return;
    }
}