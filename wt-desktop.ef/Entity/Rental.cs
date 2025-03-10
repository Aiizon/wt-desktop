using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace wt_desktop.ef.Entity;

[Table("rental")]
public class Rental : WtEntity
{
    [Required]
    [Column("billing_type_id")]
    public int BillingTypeId { get; set; }

    public BillingType BillingType { get; set; }

    [Required]
    [Column("offer_id")]
    public int OfferId { get; set; }

    public Offer Offer { get; set; }

    [Required]
    [Column("customer_id")]
    public int CustomerId { get; set; }

    public User Customer { get; set; }

    [Required]
    [Column("monthly_rent_price")]
    public double MonthlyRentPrice { get; set; }

    [Required]
    [Column("do_renew")]
    public bool DoRenew { get; set; }

    [Required]
    [Column("first_rental_date")]
    public DateTime FirstRentalDate { get; set; }

    [Column("rental_end_date")]
    public DateTime? RentalEndDate { get; set; }

    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region BillingType
        modelBuilder.Entity<Rental>()
            .HasOne(r => r.BillingType)
            .WithMany()
            .HasForeignKey(r => r.BillingTypeId)
            .HasConstraintName("FK_1619C27DAE620744")
        ;
        #endregion

        #region Offer
        modelBuilder.Entity<Rental>()
            .HasOne(r => r.Offer)
            .WithMany()
            .HasForeignKey(r => r.OfferId)
            .HasConstraintName("FK_1619C27D53C674EE")
        ;
        #endregion

        #region Customer
        modelBuilder.Entity<Rental>()
            .HasOne(r => r.Customer)
            .WithMany()
            .HasForeignKey(r => r.CustomerId)
            .HasConstraintName("FK_1619C27D9395C3F3")
        ;
        #endregion
    }

    public static IQueryable<Rental> Source(WtContext context)
    {
        return context.Rental
            .Include(r => r.BillingType)
            .Include(r => r.Offer)
            .Include(r => r.Customer)
        ;
    }
}
