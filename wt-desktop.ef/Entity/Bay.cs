using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace wt_desktop.ef.Entity;

[Table("bay")]
public class Bay : WtEntity
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Location { get; set; }

    public ICollection<Unit> Units { get; set; } = new List<Unit>();

    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bay>()
            .HasMany(b => b.Units)
            .WithOne(u => u.Bay)
            .HasForeignKey(u => u.BayId)
            .HasConstraintName("FK_DCBB0C53DF9BA23B")
        ;
    }
}