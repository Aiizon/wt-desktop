using Microsoft.EntityFrameworkCore;
using wt_desktop.ef.Entity;

namespace wt_desktop.ef;

public class WtContext: DbContext
{
    private static WtContext? _instance;

    private static readonly object _lock = new object();

    public static WtContext Instance
    {
        get
        {
            lock (_lock)
            {
                return _instance ??= new WtContext();
            }
        }
    }

    #region sets
    public DbSet<Unit>              Unit            { get; set; }

    public DbSet<Bay>               Bay             { get; set; }

    public DbSet<User>              User            { get; set; }

    public DbSet<Offer>             Offer           { get; set; }

    public DbSet<BillingType>       BillingType     { get; set; }

    public DbSet<Rental>            Rental          { get; set; }

    public DbSet<RentalUnit>        RentalUnit      { get; set; }

    public DbSet<Intervention>      Intervention    { get; set; }
    
    public DbSet<UnitIntervention> UnitIntervention { get; set; }

    public DbSet<UnitUsage>         UnitUsage       { get; set; }
    #endregion

    protected WtContext() { }

    /// <summary>
    /// Override de la méthode SaveChanges pour régénérer l'instance de la base de données
    /// </summary>
    /// <returns></returns>
    public override int SaveChanges()
    {
        int result = base.SaveChanges();
        RefreshInstance();
        return result;
    }

    /// <summary>
    /// Override de la méthode SaveChangesAsync pour régénérer l'instance de la base de données de manière asynchrone
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        int result = await base.SaveChangesAsync(cancellationToken);
        RefreshInstance();
        return result;
    }

    /// <summary>
    /// Regénère l'instance de la base de données
    /// </summary>
    private void RefreshInstance()
    {
        lock (_lock)
        {
            ChangeTracker.Clear();
            
            foreach (var entry in this.ChangeTracker.Entries().ToList())
            {
                entry.State = EntityState.Detached;
            }
        }
    }

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
        optionsBuilder.UseMySQL("server=172.18.0.1;port=3308;user=root;database=wt-app;password=root;");
        optionsBuilder.EnableSensitiveDataLogging();
    }
}