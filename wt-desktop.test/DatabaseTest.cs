using wt_desktop.ef;
using wt_desktop.ef.Entity;

namespace wt_desktop.test;

[Collection("Database")]
public class DatabaseTest: IDisposable
{
    #region Context
    private readonly WtContext       _context = WtContext.TestInstance;
    private readonly DatabaseFixture _fixture;
    
    public void Dispose()
    {
        _context.Database.EnsureDeleted();
    }

    
    public DatabaseTest(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }
    #endregion
    
    [Fact]
    public void TestConnectionSuccessful()
    {
        Assert.True(_fixture.ConnectionSuccessful);
    }
    
    [Fact]
    public void TestDatabaseCreated()
    {
        Assert.True(_fixture.DatabaseCreated);
    }

    #region User
    [Fact]
    public void TestUserCrudOperations()
    {
        var user = new User
        {
            Address    = "123 Main St",
            Email      = "test.user@worktogether.com",
            FirstName  = "Test",
            LastName   = "User",
            IsVerified = true,
            Password   = AuthProvider.Instance.HashPassword("password"),
            Roles      = "ROLE_USER",
            Type       = "user",
        };
        
        _context.Add(user);
        _context.SaveChanges();
        
        Assert.NotEqual(0, user.Id);

        user.FirstName = "Updated";
        _context.Update(user);
        _context.SaveChanges();
        
        Assert.Equal("Updated", _context.User.Find(user.Id)!.FirstName);
        
        _context.Remove(user);
        _context.SaveChanges();
        
        Assert.Null(_context.User.Find(user.Id));
    }
    #endregion
    
    #region Unit
    [Fact]
    public void TestUnitCrudOperations()
    {
        var unit = new Unit
        {
            Name = "Test Unit",
            Status = Unit.EUnitStatus.OK,
            IsStarted = true,
        };
        
        _context.Unit.Add(unit);
        _context.SaveChanges();
        
        Assert.NotEqual(0, unit.Id);

        unit.Name = "Updated Unit";
        _context.Unit.Update(unit);
        _context.SaveChanges();
        
        Assert.Equal("Updated Unit", _context.Unit.Find(unit.Id)!.Name);
        
        _context.Unit.Remove(unit);
        _context.SaveChanges();
        
        Assert.Null(_context.Unit.Find(unit.Id));
    }
    
    [Fact]
    public void TestUnitBayForeignKey()
    {
        var bay = new Bay
        {
            Name     = "Test Bay",
            Location = "Test Location",
        };
        
        var unit = new Unit
        {
            Name      = "Test Unit",
            Status    = Unit.EUnitStatus.OK,
            IsStarted = true,
            Bay       = bay,
        };
        
        _context.Bay.Add(bay);
        _context.Unit.Add(unit);
        _context.SaveChanges();
        
        Assert.NotEqual(0, bay.Id);
        Assert.NotEqual(0, unit.Id);
        
        Assert.Equal(bay.Id, unit.Bay.Id);
    }
    
    [Fact]
    public void TestUnitUsageForeignKey()
    {
        var usage = new UnitUsage
        {
            Name  = "Test Usage",
            Color = "#FF0000",
        };
        
        var unit = new Unit
        {
            Name      = "Test Unit",
            Status    = Unit.EUnitStatus.OK,
            IsStarted = true,
            UnitUsage = usage,
        };
        
        _context.UnitUsage.Add(usage);
        _context.Unit.Add(unit);
        _context.SaveChanges();
        
        Assert.NotEqual(0, usage.Id);
        Assert.NotEqual(0, unit.Id);
        
        Assert.Equal(usage.Id, unit.UnitUsage.Id);
    }
    #endregion
    
    #region Bay
    [Fact]
    public void TestBayCrudOperations()
    {
        var bay = new Bay
        {
            Name     = "Test Bay",
            Location = "Test Location",
        };
        
        _context.Bay.Add(bay);
        _context.SaveChanges();
        
        Assert.NotEqual(0, bay.Id);

        bay.Name = "Updated Bay";
        _context.Bay.Update(bay);
        _context.SaveChanges();
        
        Assert.Equal("Updated Bay", _context.Bay.Find(bay.Id)!.Name);
        
        _context.Bay.Remove(bay);
        _context.SaveChanges();
        
        Assert.Null(_context.Bay.Find(bay.Id));
    }
    
    [Fact]
    public void TestBayUnitForeignKey()
    {
        var bay = new Bay
        {
            Name     = "Test Bay",
            Location = "Test Location",
        };
        
        var unit = new Unit
        {
            Name      = "Test Unit",
            Status    = Unit.EUnitStatus.OK,
            IsStarted = true,
            Bay       = bay,
        };
        
        _context.Bay.Add(bay);
        _context.Unit.Add(unit);
        _context.SaveChanges();
        
        Assert.NotEqual(0, bay.Id);
        Assert.NotEqual(0, unit.Id);
        
        Assert.Equal(bay.Id, unit.Bay.Id);
    }
    #endregion
    
    #region Offer
    [Fact]
    public void TestOfferCrudOperations()
    {
        var offer = new Offer
        {
            Name             = "Test Offer",
            MonthlyRentPrice = 100.0,
            Availability     = "100%",
            IsActive         = true,
            MaxUnits         = 12,
            Bandwidth        = "100 Mbps",
        };
        
        _context.Offer.Add(offer);
        _context.SaveChanges();
        
        Assert.NotEqual(0, offer.Id);

        offer.Name = "Updated Offer";
        _context.Offer.Update(offer);
        _context.SaveChanges();
        
        Assert.Equal("Updated Offer", _context.Offer.Find(offer.Id)!.Name);
        
        _context.Offer.Remove(offer);
        _context.SaveChanges();
        
        Assert.Null(_context.Offer.Find(offer.Id));
    }
    #endregion
    
    #region Rental
    [Fact]
    public void TestRentalCrudOperations()
    {
        var rental = new Rental
        {
            FirstRentalDate  = DateTime.Now,
            RentalEndDate    = DateTime.Now.AddMonths(1),
            MonthlyRentPrice = 100.0,
            DoRenew          = false,
        };
        
        _context.Rental.Add(rental);
        _context.SaveChanges();
        
        Assert.NotEqual(0, rental.Id);

        rental.MonthlyRentPrice = 150.0;
        _context.Rental.Update(rental);
        _context.SaveChanges();
        
        Assert.Equal(150.0, _context.Rental.Find(rental.Id)!.MonthlyRentPrice);
        
        _context.Rental.Remove(rental);
        _context.SaveChanges();
        
        Assert.Null(_context.Rental.Find(rental.Id));
    }
    
    [Fact]
    public void TestRentalUnitForeignKey()
    {
        var rental = new Rental
        {
            FirstRentalDate  = DateTime.Now,
            RentalEndDate    = DateTime.Now.AddMonths(1),
            MonthlyRentPrice = 100.0,
            DoRenew          = false,
        };
        
        var unit = new Unit
        {
            Name      = "Test Unit",
            Status    = Unit.EUnitStatus.OK,
            IsStarted = true,
        };
        
        _context.Rental.Add(rental);
        _context.Unit.Add(unit);
        _context.SaveChanges();
        
        Assert.NotEqual(0, rental.Id);
        Assert.NotEqual(0, unit.Id);
        
        var rentalUnit = new RentalUnit
        {
            RentalId = rental.Id,
            UnitId   = unit.Id,
        };
        
        _context.RentalUnit.Add(rentalUnit);
        _context.SaveChanges();
        
        Assert.NotNull(_context.RentalUnit.First(ru => ru.RentalId == rental.Id && ru.UnitId == unit.Id));
    }
    
    [Fact]
    public void TestRentalOfferForeignKey()
    {
        var offer = new Offer
        {
            Name             = "Test Offer",
            MonthlyRentPrice = 100.0,
            Availability     = "100%",
            IsActive         = true,
            MaxUnits         = 12,
            Bandwidth        = "100 Mbps",
        };
        
        var rental = new Rental
        {
            FirstRentalDate  = DateTime.Now,
            RentalEndDate    = DateTime.Now.AddMonths(1),
            MonthlyRentPrice = 100.0,
            DoRenew          = false,
            Offer            = offer,
        };
        
        _context.Offer.Add(offer);
        _context.Rental.Add(rental);
        _context.SaveChanges();
        
        Assert.NotEqual(0, offer.Id);
        Assert.NotEqual(0, rental.Id);
        
        Assert.Equal(offer.Id, rental.Offer.Id);
    }
    #endregion
    
    #region Intervention
    [Fact]
    public void TestInterventionCrudOperations()
    {
        var intervention = new Intervention
        {
            Comment     = "Test Intervention",
            StartDate   = DateTime.Now,
            EndDate     = DateTime.Now.AddHours(1),
        };
        
        _context.Intervention.Add(intervention);
        _context.SaveChanges();
        
        Assert.NotEqual(0, intervention.Id);

        intervention.Comment = "Updated Intervention";
        _context.Intervention.Update(intervention);
        _context.SaveChanges();
        
        Assert.Equal("Updated Intervention", _context.Intervention.Find(intervention.Id)!.Comment);
        
        _context.Intervention.Remove(intervention);
        _context.SaveChanges();
        
        Assert.Null(_context.Intervention.Find(intervention.Id));
    }
    
    [Fact]
    public void TestInterventionUnitForeignKey()
    {
        var unit = new Unit
        {
            Name      = "Test Unit",
            Status    = Unit.EUnitStatus.OK,
            IsStarted = true,
        };
        
        var intervention = new Intervention
        {
            Comment     = "Test Intervention",
            StartDate   = DateTime.Now,
            EndDate     = DateTime.Now.AddHours(1),
        };
        
        _context.Unit.Add(unit);
        _context.Intervention.Add(intervention);
        _context.SaveChanges();
        
        Assert.NotEqual(0, unit.Id);
        Assert.NotEqual(0, intervention.Id);

        var unitIntervention = new UnitIntervention
        {
            UnitId         = unit.Id,
            InterventionId = intervention.Id,
        };
        
        _context.UnitIntervention.Add(unitIntervention);
        _context.SaveChanges();
        
        Assert.NotNull(_context.UnitIntervention.First(ui => ui.UnitId == unit.Id && ui.InterventionId == intervention.Id));
    }
    #endregion
}