using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace wt_desktop.ef.Entity;

[Table("rental_unit")]
public class RentalUnit: WtEntity
{
    public virtual Rental? Rental { get; set; }

    [Required]
    [Column("rental_id")]
    public int RentalId { get; set; }

    public virtual Unit? Unit { get; set; }

    [Required]
    [Column("unit_id")]
    public int UnitId { get; set; }

    public override string DisplayText => $"{RentalId} {UnitId}";

    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Rental
        modelBuilder.Entity<RentalUnit>()
            .HasOne(ru => ru.Rental)
            .WithMany()
            .HasForeignKey(ru => ru.RentalId)
            .HasConstraintName("FK_53F75393A7CF2329")
            .IsRequired(true)
        ;
        #endregion

        #region Unit
        modelBuilder.Entity<RentalUnit>()
            .HasOne(ru => ru.Unit)
            .WithMany()
            .HasForeignKey(ru => ru.UnitId)
            .HasConstraintName("FK_53F75393F8BD700D")
            .IsRequired(true)
        ;
        #endregion

        modelBuilder.Entity<RentalUnit>().HasKey(
            ru => new { ru.RentalId, ru.UnitId }
        );
    }
}
