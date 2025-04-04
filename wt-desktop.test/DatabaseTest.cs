using wt_desktop.ef;
using wt_desktop.ef.Entity;

namespace wt_desktop.test;

[Collection("Database")]
public class DatabaseTest: IDisposable
{
    #region Context
    private readonly WtContext       _Context = WtContext.TestInstance;
    private readonly DatabaseFixture _Fixture;
    
    public void Dispose()
    {
        _Context.Database.EnsureDeleted();
    }

    
    public DatabaseTest(DatabaseFixture fixture)
    {
        _Fixture = fixture;
    }
    #endregion
    
    [Fact]
    public void TestConnectionSuccessful()
    {
        Assert.True(_Fixture.ConnectionSuccessful);
    }
    
    [Fact]
    public void TestDatabaseCreated()
    {
        Assert.True(_Fixture.DatabaseCreated);
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
            isVerified = true,
            Password   = AuthProvider.Instance.HashPassword("password"),
            Roles      = "ROLE_USER",
            Type       = "user",
        };
        
        _Context.Add(user);
        _Context.SaveChanges();
        
        Assert.NotEqual(0, user.Id);

        user.FirstName = "Updated";
        _Context.Update(user);
        _Context.SaveChanges();
        
        Assert.Equal("Updated", _Context.User.Find(user.Id)!.FirstName);
        
        _Context.Remove(user);
        _Context.SaveChanges();
        
        Assert.Null(_Context.User.Find(user.Id));
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
        
        _Context.Unit.Add(unit);
        _Context.SaveChanges();
        
        Assert.NotEqual(0, unit.Id);

        unit.Name = "Updated Unit";
        _Context.Unit.Update(unit);
        _Context.SaveChanges();
        
        Assert.Equal("Updated Unit", _Context.Unit.Find(unit.Id)!.Name);
        
        _Context.Unit.Remove(unit);
        _Context.SaveChanges();
        
        Assert.Null(_Context.Unit.Find(unit.Id));
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
        
        _Context.Bay.Add(bay);
        _Context.Unit.Add(unit);
        _Context.SaveChanges();
        
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
        
        _Context.UnitUsage.Add(usage);
        _Context.Unit.Add(unit);
        _Context.SaveChanges();
        
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
        
        _Context.Bay.Add(bay);
        _Context.SaveChanges();
        
        Assert.NotEqual(0, bay.Id);

        bay.Name = "Updated Bay";
        _Context.Bay.Update(bay);
        _Context.SaveChanges();
        
        Assert.Equal("Updated Bay", _Context.Bay.Find(bay.Id)!.Name);
        
        _Context.Bay.Remove(bay);
        _Context.SaveChanges();
        
        Assert.Null(_Context.Bay.Find(bay.Id));
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
        
        _Context.Bay.Add(bay);
        _Context.Unit.Add(unit);
        _Context.SaveChanges();
        
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
        
        _Context.Offer.Add(offer);
        _Context.SaveChanges();
        
        Assert.NotEqual(0, offer.Id);

        offer.Name = "Updated Offer";
        _Context.Offer.Update(offer);
        _Context.SaveChanges();
        
        Assert.Equal("Updated Offer", _Context.Offer.Find(offer.Id)!.Name);
        
        _Context.Offer.Remove(offer);
        _Context.SaveChanges();
        
        Assert.Null(_Context.Offer.Find(offer.Id));
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
        
        _Context.Rental.Add(rental);
        _Context.SaveChanges();
        
        Assert.NotEqual(0, rental.Id);

        rental.MonthlyRentPrice = 150.0;
        _Context.Rental.Update(rental);
        _Context.SaveChanges();
        
        Assert.Equal(150.0, _Context.Rental.Find(rental.Id)!.MonthlyRentPrice);
        
        _Context.Rental.Remove(rental);
        _Context.SaveChanges();
        
        Assert.Null(_Context.Rental.Find(rental.Id));
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
        
        _Context.Rental.Add(rental);
        _Context.Unit.Add(unit);
        _Context.SaveChanges();
        
        Assert.NotEqual(0, rental.Id);
        Assert.NotEqual(0, unit.Id);
        
        var rentalUnit = new RentalUnit
        {
            RentalId = rental.Id,
            UnitId   = unit.Id,
        };
        
        _Context.RentalUnit.Add(rentalUnit);
        _Context.SaveChanges();
        
        Assert.NotNull(_Context.RentalUnit.First(ru => ru.RentalId == rental.Id && ru.UnitId == unit.Id));
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
        
        _Context.Offer.Add(offer);
        _Context.Rental.Add(rental);
        _Context.SaveChanges();
        
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
        
        _Context.Intervention.Add(intervention);
        _Context.SaveChanges();
        
        Assert.NotEqual(0, intervention.Id);

        intervention.Comment = "Updated Intervention";
        _Context.Intervention.Update(intervention);
        _Context.SaveChanges();
        
        Assert.Equal("Updated Intervention", _Context.Intervention.Find(intervention.Id)!.Comment);
        
        _Context.Intervention.Remove(intervention);
        _Context.SaveChanges();
        
        Assert.Null(_Context.Intervention.Find(intervention.Id));
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
        
        _Context.Unit.Add(unit);
        _Context.Intervention.Add(intervention);
        _Context.SaveChanges();
        
        Assert.NotEqual(0, unit.Id);
        Assert.NotEqual(0, intervention.Id);

        var unitIntervention = new UnitIntervention
        {
            UnitId         = unit.Id,
            InterventionId = intervention.Id,
        };
        
        _Context.UnitIntervention.Add(unitIntervention);
        _Context.SaveChanges();
        
        Assert.NotNull(_Context.UnitIntervention.First(ui => ui.UnitId == unit.Id && ui.InterventionId == intervention.Id));
    }
    #endregion
}