using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace wt_desktop.ef.Entity;

[Table("bay")]
public class Bay : WtEntity
{
    [Required]
    [Column("name")]
    public string Name { get; set; }

    [Required]
    [Column("location")]
    public string Location { get; set; }

    public virtual IQueryable<Unit> Units(WtContext context)
        => context.Unit.Where(u => u.Bay.Id == Id);

    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        return;
    }
}