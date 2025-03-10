using Microsoft.EntityFrameworkCore;
using wt_desktop.ef.Entity;

namespace wt_desktop.ef;

public class WtContext: DbContext
{
    #region sets
    public DbSet<Unit>          Unit        { get; set; }

    public DbSet<Bay>           Bay         { get; set; }

    public DbSet<User>          User        { get; set; }

    public DbSet<Offer>         Offer       { get; set; }

    public DbSet<BillingType>   BillingType { get; set; }

    public DbSet<Rental>        Rental      { get; set; }

    public DbSet<Intervention> Intervention { get; set; }

    public DbSet<UnitUsage>    UnitUsage    { get; set; }
    #endregion

    public WtContext() { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new User()        .OnModelCreating(modelBuilder);
        new UnitUsage()   .OnModelCreating(modelBuilder);
        new Bay()         .OnModelCreating(modelBuilder);
        new Unit()        .OnModelCreating(modelBuilder);
        new Offer()       .OnModelCreating(modelBuilder);
        new BillingType() .OnModelCreating(modelBuilder);
        new Intervention().OnModelCreating(modelBuilder);
        new Rental()      .OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL("server=127.0.0.1;port=3308;user=root;database=wt-app;password=root;");
    }
}