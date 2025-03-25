using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace wt_desktop.ef.Entity;

[Table("rental")]
public class Rental : WtIdentityEntity
{
    [Required]
    [Column("billing_type_id")]
    public BillingType? BillingType { get; set; }

    [Required]
    [Column("offer_id")]
    public Offer? Offer { get; set; }

    [Required]
    [Column("customer_id")]
    public User? Customer { get; set; }

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

    public virtual IQueryable<Unit?> Units
        => RentalUnit
            .Source()
            .Where(ru => ru.RentalId == Id)
            .Select(ru => ru.Unit);

    public override string DisplayText => $"{Offer?.DisplayText}, {Customer?.DisplayText}";

    public override string SearchText => $"{BillingType?.DisplayText} {Offer?.DisplayText} {Customer?.DisplayText} {MonthlyRentPrice} {DoRenew} {FirstRentalDate} {RentalEndDate}";

    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region BillingType
        modelBuilder.Entity<Rental>()
            .HasOne(r => r.BillingType)
            .WithMany()
            .HasForeignKey("billing_type_id")
            .HasConstraintName("FK_1619C27DAE620744")
            .IsRequired(true)
        ;
        #endregion

        #region Offer
        modelBuilder.Entity<Rental>()
            .HasOne(r => r.Offer)
            .WithMany()
            .HasForeignKey("offer_id")
            .HasConstraintName("FK_1619C27D53C674EE")
            .IsRequired(true)
        ;
        #endregion

        #region Customer
        modelBuilder.Entity<Rental>()
            .HasOne(r => r.Customer)
            .WithMany()
            .HasForeignKey("customer_id")
            .HasConstraintName("FK_1619C27D9395C3F3")
            .IsRequired(true)
        ;
        #endregion
    }

    public static IQueryable<Rental> Source()
    {
        return WtContext.Instance.Rental
            .Include(r => r.BillingType)
            .Include(r => r.Offer)
            .Include(r => r.Customer)
        ;
    }
}
