using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace wt_desktop.ef.Entity;

[Table("unit_intervention")]
public class UnitIntervention: WtEntity
{
    public Intervention? Intervention { get; set; }
    
    [Required]
    [Column("intervention_id")]
    public int InterventionId { get; set; }
    
    public Unit? Unit { get; set; }
    
    [Required]
    [Column("unit_id")]
    public int UnitId { get; set; }
    
    public override string DisplayText => $"{InterventionId} {UnitId}";
    
    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Intervention
        modelBuilder.Entity<UnitIntervention>()
            .HasOne(ui => ui.Intervention)
            .WithMany()
            .HasForeignKey(ui => ui.InterventionId)
            .HasConstraintName("FK_38EF63C8EAE3863")
            .IsRequired(true)
        ;
        #endregion
        
        #region Unit
        modelBuilder.Entity<UnitIntervention>()
            .HasOne(ui => ui.Unit)
            .WithMany()
            .HasForeignKey(ui => ui.UnitId)
            .HasConstraintName("FK_38EF63CF8BD700D")
            .IsRequired(true)
        ;
        #endregion
        
        modelBuilder.Entity<UnitIntervention>().HasKey(
            ui => new { ui.InterventionId, ui.UnitId }
        );
    }
    
    public static IQueryable<UnitIntervention> Source()
    {
        return WtContext.Instance.UnitIntervention
            .Include(ui => ui.Intervention)
            .Include(ui => ui.Unit);
    }
}