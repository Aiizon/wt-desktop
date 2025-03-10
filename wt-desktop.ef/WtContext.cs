using Microsoft.EntityFrameworkCore;
using wt_desktop.ef.Entity;

namespace wt_desktop.ef;

public class WtContext: DbContext
{
    public WtContext(): base(GetDbContextOptions()) { }

    public DbSet<Unit>          Unit        { get; set; }

    public DbSet<Bay>           Bay         { get; set; }

    public DbSet<User>          User        { get; set; }

    public DbSet<Offer>         Offer       { get; set; }

    public DbSet<BillingType>   BillingType { get; set; }

    public DbSet<Rental>        Rental      { get; set; }

    public DbSet<Intervention> Intervention { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new Unit()        .OnModelCreating(modelBuilder);
        new Bay()         .OnModelCreating(modelBuilder);
        new User()        .OnModelCreating(modelBuilder);
        new Offer()       .OnModelCreating(modelBuilder);
        new BillingType() .OnModelCreating(modelBuilder);
        new Rental()      .OnModelCreating(modelBuilder);
        new Intervention().OnModelCreating(modelBuilder);
    }

    public static DbContextOptions GetDbContextOptions()
    {
        var optionsBuilder = new DbContextOptionsBuilder<WtContext>();
        optionsBuilder.UseMySQL("mysql://root:root@localhost:3308/wt-app?serverVersion=8.0.32&charset=utf8mb4");
        return optionsBuilder.Options;
    }
}