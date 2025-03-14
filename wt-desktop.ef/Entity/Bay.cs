using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace wt_desktop.ef.Entity;

[Table("bay")]
public class Bay : WtIdentityEntity
{
    [Required]
    [Column("name")]
    public string Name { get; set; }

    [Required]
    [Column("location")]
    public string Location { get; set; }

    public virtual IQueryable<Unit> Units()
        => WtContext.Instance.Unit.Where(u => u.Bay.Id == Id);

    public int Size
        => Units().ToList().Count;

    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        return;
    }
}