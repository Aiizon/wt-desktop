using Microsoft.EntityFrameworkCore;
using wt_desktop.ef.Entity;

namespace wt_desktop.ef;

public class WtContext: DbContext
{
    #region sets
    public DbSet<Unit>          Unit         { get; set; }

    public DbSet<Bay>           Bay          { get; set; }

    public DbSet<User>          User         { get; set; }

    public DbSet<Offer>         Offer        { get; set; }

    public DbSet<BillingType>   BillingType  { get; set; }

    public DbSet<Rental>        Rental       { get; set; }

    public DbSet<Intervention>  Intervention { get; set; }

    public DbSet<UnitUsage>     UnitUsage    { get; set; }
    #endregion

    public WtContext() { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        IEnumerable<Type>? entityTypes = typeof(WtEntity).Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(WtEntity)) && !t.IsAbstract);

        foreach (Type type in entityTypes)
        {
            var instance = Activator.CreateInstance(type) as WtEntity;
            instance?.OnModelCreating(modelBuilder);
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL("server=127.0.0.1;port=3308;user=root;database=wt-app;password=root;");
    }
}