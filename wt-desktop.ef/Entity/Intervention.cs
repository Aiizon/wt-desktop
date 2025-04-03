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
    public DateTime? StartDate { get; set; }

    [Column("end_date")]
    public DateTime? EndDate { get; set; }
    
    [NotMapped]
    public virtual IQueryable<UnitIntervention> UnitInterventions
        => WtContext.Instance.UnitIntervention
            .Where(ui => ui.InterventionId == Id)
            .Select(ui => ui);
    
    [NotMapped]
    public virtual IQueryable<Unit?> Units
        => WtContext.Instance.Set<UnitIntervention>()
            .Where(ui => ui.InterventionId == Id)
            .Select(ui => ui.Unit);
    
    [NotMapped]
    public string UnitsText
        => string.Join(", ", Units.Select(u => $"{u!.Name} ({u.Bay!.Name ?? "N/A"})"));

    [NotMapped]
    public virtual IQueryable<Bay?> Bays
        => WtContext.Instance.Set<UnitIntervention>()
            .Where(ui => ui.InterventionId == Id)
            .Select(ui => ui.Unit)
            .Select(u => u!.Bay);
    
    [NotMapped]
    public string BaysText
        => string.Join(", ", Bays.Select(b => b!.DisplayText));

    public override string DisplayText => Comment;

    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        return;
    }
}
