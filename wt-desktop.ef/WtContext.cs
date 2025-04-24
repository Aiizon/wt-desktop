using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using wt_desktop.ef.Entity;

namespace wt_desktop.ef;

public class WtContext: DbContext
{
    #region Singleton
    private static WtContext? _instance;

    private static readonly Lock Lock = new();

    /// <summary>
    /// Instance de la base de données
    /// </summary>
    public static WtContext Instance
    {
        get
        {
            lock (Lock)
            {
                return _instance ??= new WtContext();
            }
        }
    }
    
    /// <summary>
    /// Instance de la base de données pour les tests
    /// </summary>
    public static WtContext TestInstance
    {
        get
        {
            lock (Lock)
            {
                _instance = GetTestInstance();
                return _instance;
            }
        }
    }
    
    protected WtContext() { }
    #endregion
    
    private WtContext(DbContextOptions<WtContext> options) : base(options) { }
    
    /// <summary>
    /// Génère une instance de la base de données en RAM pour les tests
    /// </summary>
    /// <returns>instance</returns>
    private static WtContext GetTestInstance()
    {
        return new WtContext
        (
            new DbContextOptionsBuilder<WtContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options
        );
    }

    #region Sets
    public DbSet<Unit>              Unit            { get; set; }

    public DbSet<Bay>               Bay             { get; set; }

    public DbSet<User>              User            { get; set; }

    public DbSet<Offer>             Offer           { get; set; }

    public DbSet<BillingType>       BillingType     { get; set; }

    public DbSet<Rental>            Rental          { get; set; }

    public DbSet<RentalUnit>        RentalUnit      { get; set; }

    public DbSet<Intervention>      Intervention    { get; set; }
    
    public DbSet<UnitIntervention>  UnitIntervention { get; set; }

    public DbSet<UnitUsage>         UnitUsage       { get; set; }
    #endregion
    
    /// <summary>
    /// Override de la méthode SaveChanges pour régénérer l'instance de la base de données
    /// </summary>
    /// <returns></returns>
    public override int SaveChanges()
    {
        try
        {
            int result = base.SaveChanges();
            RefreshInstance();
            return result;
        }
        catch (Exception e)
        {
            throw new Exception("Erreur lors de la sauvegarde des changements dans la base de données.", e);
        }
    }

    /// <summary>
    /// Override de la méthode SaveChangesAsync pour régénérer l'instance de la base de données de manière asynchrone
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            int result = await base.SaveChangesAsync(cancellationToken);
            RefreshInstance();
            return result;
        }
        catch (Exception e)
        {
            throw new Exception("Erreur lors de la sauvegarde des changements dans la base de données.", e);
        }
    }

    /// <summary>
    /// Regénère l'instance de la base de données
    /// </summary>
    private void RefreshInstance()
    {
        lock (Lock)
        {
            try
            {
                var oldInstance = _instance;

                _instance = new WtContext();

                if (oldInstance != null && !ReferenceEquals(oldInstance, this))
                {
                    foreach (var entry in oldInstance.ChangeTracker.Entries().ToList())
                    {
                        entry.State = EntityState.Detached;
                    }
                    
                    oldInstance.Dispose();
                }
                else
                {
                    foreach (var entry in ChangeTracker.Entries().ToList())
                    {
                        entry.State = EntityState.Detached;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Erreur lors de la sauvegarde des changements dans la base de données.", e);
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        IEnumerable<Type> entityTypes = typeof(WtEntity).Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(WtEntity)) && !t.IsAbstract);

        foreach (Type type in entityTypes)
        {
            var instance = Activator.CreateInstance(type) as WtEntity;
            instance?.OnModelCreating(modelBuilder);
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        try
        {
            Env.Load(Path.Combine(AppContext.BaseDirectory, ".env"));
            
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseMySQL(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? throw new Exception("DB_CONNECTION_STRING introuvable dans le fichier .env."));
            optionsBuilder.EnableSensitiveDataLogging();
        }
        catch (Exception e)
        {
            throw new Exception("Erreur lors de la configuration de la base de données.", e);
        }
    }
}