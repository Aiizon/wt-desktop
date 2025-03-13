using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace wt_desktop.ef.Entity;

[Table("rental_unit")]
public class RentalUnit
{
    [Key]
    [Required]
    [Column("rental_id")]
    public Rental Rental { get; set; }

    [Key]
    [Required]
    [Column("unit_id")]
    public Unit Unit { get; set; }

    public void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Rental
        modelBuilder.Entity<RentalUnit>()
            .HasOne(ru => ru.Rental)
            .WithMany()
            .HasForeignKey("rental_id")
            .HasConstraintName("FK_53F75393A7CF2329")
            .IsRequired(true)
        ;
        #endregion

        #region Unit
        modelBuilder.Entity<RentalUnit>()
            .HasOne(ru => ru.Unit)
            .WithMany()
            .HasForeignKey("unit_id")
            .HasConstraintName("FK_53F75393F8BD700D")
            .IsRequired(true)
        ;
        #endregion
    }

    public static IQueryable<RentalUnit> Source(WtContext context)
    {
        return context.RentalUnit
            .Include(ru => ru.Rental)
            .Include(ru => ru.Unit)
        ;
    }
}
