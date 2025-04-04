using wt_desktop.ef;

namespace wt_desktop.test;

public class DatabaseFixture: IDisposable
{
    private readonly WtContext _Context = WtContext.TestInstance;
    public bool ConnectionSuccessful { get; private set; }
    public bool DatabaseCreated      { get; private set; }
    
    public DatabaseFixture()
    {
        ConnectionSuccessful = _Context.Database.CanConnect();

        if (ConnectionSuccessful)
        {
            DatabaseCreated = _Context.Database.EnsureCreated();
        }
    }
    
    public void Dispose()
    {
        _Context.Database.EnsureDeleted();
    }
}

[CollectionDefinition("Database")]
public class DatabaseCollection: ICollectionFixture<DatabaseFixture> { }