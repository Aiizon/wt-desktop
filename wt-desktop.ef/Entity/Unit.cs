using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace wt_desktop.ef.Entity;

[Table("unit")]
public class Unit : WtEntity
{
    public enum EUnitStatus
    {
        OK          = 1,
        KO          = 2,
        Maintenance = 3
    }

    [Required]
    [Column("unit_usage_id")]
    public int UnitUsageId { get; set; }

    public UnitUsage UnitUsage { get; set; }

    [Required]
    [Column("bay_id")]
    public int BayId { get; set; }

    public Bay Bay { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; }

    [Required]
    [Column("is_started")]
    public bool IsStarted { get; set; }

    [Required]
    [Column("status")]
    public EUnitStatus Status { get; set; }

    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Bay
        modelBuilder.Entity<Unit>()
            .HasOne(u => u.Bay)
            .WithMany()
            .HasForeignKey(u => u.BayId)
            .HasConstraintName("FK_DCBB0C53DF9BA23B")
        ;
        #endregion

        #region UnitUsage
        modelBuilder.Entity<Unit>()
            .HasOne(u => u.UnitUsage)
            .WithMany()
            .HasForeignKey(u => u.UnitUsageId)
            .HasConstraintName("FK_DCBB0C53546E0C08")
        ;
        #endregion
    }

    /// <summary>
    /// Returns the Unit with Bay and UnitUsage included.
    /// </summary>
    /// <param name="context">Context</param>
    public static IQueryable<Unit> Source(WtContext context)
    {
        return context.Unit
            .Include(u => u.Bay)
            .Include(u => u.UnitUsage)
        ;
    }
}
