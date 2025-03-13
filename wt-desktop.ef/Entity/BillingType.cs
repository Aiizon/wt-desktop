using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace wt_desktop.ef.Entity;

[Table("billing_type")]
public class BillingType : WtIdentityEntity
{
    [Required]
    [Column("months")]
    public int Months { get; set; }

    [Required]
    [Column("discount_over_monthly")]
    public double DiscountOverMonthly { get; set; }

    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        return;
    }
}
