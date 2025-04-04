using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace wt_desktop.ef.Entity;

[Table("offer")]
public class Offer : WtIdentityEntity
{
    [Required]
    [Column("name")]
    public string Name { get; set; }

    [Required]
    [Column("max_units")]
    public int? MaxUnits { get; set; }

    [Required]
    [Column("availability")]
    public string Availability { get; set; }

    [Required]
    [Column("monthly_rent_price")]
    public double? MonthlyRentPrice { get; set; }

    [Required]
    [Column("bandwidth")]
    public string Bandwidth { get; set; }

    [Required]
    [Column("is_active")]
    public bool IsActive { get; set; }

    public override string DisplayText => Name;

    public override string SearchText => $"{Name} {MaxUnits} {Availability} {MonthlyRentPrice} {Bandwidth}";

    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        return;
    }
}