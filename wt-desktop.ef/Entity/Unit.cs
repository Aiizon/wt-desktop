using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace wt_desktop.ef.Entity;

[Table("unit")]
public class Unit : WtIdentityEntity
{
    public enum EUnitStatus
    {
        OK          = 1,
        KO          = 2,
        Maintenance = 3
    }

    [Column("unit_usage_id")]
    public UnitUsage? UnitUsage { get; set; } = null;

    [Required]
    [Column("bay_id")]
    public Bay? Bay { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; }

    [Required]
    [Column("is_started")]
    public bool IsStarted { get; set; }

    [Required]
    [Column("status")]
    public EUnitStatus Status { get; set; }
    
    [NotMapped]
    public string StatusText => Status switch
    {
        EUnitStatus.OK          => "OK",
        EUnitStatus.KO          => "KO",
        EUnitStatus.Maintenance => "Maintenance",
        _                       => "Inconnu"
    };

    public virtual IQueryable<Rental?> Rentals
        => RentalUnit
            .Source()
            .Where(ru => ru.UnitId == Id)
            .Select(ru => ru.Rental);

    public override string DisplayText => Name;
    
    public string DisplayTextWithBay => $"{Name} ({Bay!.Name})";

    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Bay
        modelBuilder.Entity<Unit>()
            .HasOne(u => u.Bay)
            .WithMany()
            .HasForeignKey("bay_id")
            .HasConstraintName("FK_DCBB0C53DF9BA23B")
            .IsRequired(true)
        ;
        #endregion

        #region UnitUsage
        modelBuilder.Entity<Unit>()
            .HasOne(u => u.UnitUsage)
            .WithMany()
            .HasForeignKey("unit_usage_id")
            .HasConstraintName("FK_DCBB0C53546E0C08")
            .IsRequired(false)
        ;
        #endregion
    }

    /// <summary>
    /// Returns the Unit with Bay and UnitUsage included.
    /// </summary>
    /// <param name="context">Context</param>
    public static IQueryable<Unit> Source()
    {
        return WtContext.Instance.Unit
            .Include(u => u.Bay)
            .Include(u => u.UnitUsage)
        ;
    }
}
