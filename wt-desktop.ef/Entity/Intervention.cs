using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace wt_desktop.ef.Entity;

[Table("intervention")]
public class Intervention : WtIdentityEntity
{
    [Required]
    [Column("comment")]
    public string Comment { get; set; }

    [Required]
    [Column("start_date")]
    public DateTime StartDate { get; set; }

    [Column("end_date")]
    public DateTime? EndDate { get; set; }

    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        return;
    }
}
