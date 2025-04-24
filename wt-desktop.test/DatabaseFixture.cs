using wt_desktop.ef;

namespace wt_desktop.test;

public class DatabaseFixture: IDisposable
{
    private readonly WtContext _context = WtContext.TestInstance;
    public bool ConnectionSuccessful { get; private set; }
    public bool DatabaseCreated      { get; private set; }
    
    public DatabaseFixture()
    {
        ConnectionSuccessful = _context.Database.CanConnect();

        if (ConnectionSuccessful)
        {
            DatabaseCreated = _context.Database.EnsureCreated();
        }
    }
    
    public void Dispose()
    {
        _context.Database.EnsureDeleted();
    }
}

[CollectionDefinition("Database")]
public class DatabaseCollection: ICollectionFixture<DatabaseFixture> { }