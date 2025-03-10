using wt_desktop.ef;
using wt_desktop.ef.Entity;

namespace wt_desktop.con;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string reference = "teehee";
            WtContext context = new WtContext();
            ConsoleTools.Display("Database connection established.", "debug");

            ConsoleTools.Pause(context.Database.CanConnect() ? "Database connection successful." : "Database connection failed.");

            Bay  bay  = GenerateBay(reference);
            Unit unit = GenerateUnit(reference, bay);

            SearchBay(reference);
            SearchUnit(reference);
        }
        catch (Exception e)
        {
            ConsoleTools.PrintError(e);
            throw;
        }
    }

    #region generate
    static Bay GenerateBay(string reference)
    {
        WtContext context = new WtContext();

        Bay testBay = new Bay
        {
            Name = reference,
            Location = "Test Location"
        };

        context.Bay.Add(testBay);
        context.SaveChanges();

        ConsoleTools.Pause("Bay added.");

        return testBay;
    }

    static Unit GenerateUnit(string reference, Bay bay)
    {
        WtContext context = new WtContext();

        Unit testUnit = new Unit
        {
            Name = reference,
            IsStarted = true,
            Status = Unit.EUnitStatus.OK,
            BayId = bay.Id,
        };

        context.Unit.Add(testUnit);
        context.SaveChanges();

        ConsoleTools.Pause("Unit added.");

        return testUnit;
    }
    #endregion

    #region display
    static void SearchBay(string reference)
    {
        WtContext context = new WtContext();

        Bay? bay = context.Bay.FirstOrDefault(u => u.Name == reference);

        if (bay != null)
        {
            ConsoleTools.Display($"Bay found: {bay.Id} ({bay.Units.Count} units)");
        }
        else
        {
            throw new Exception("Bay not found.");
        }
        ConsoleTools.Pause("Bay shown.");
    }

    static void SearchUnit(string reference)
    {
        WtContext context = new WtContext();

        Unit? unit = context.Unit.FirstOrDefault(u => u.Name == reference);

        if (unit != null)
        {
            ConsoleTools.Display($"Unit found: {unit.Id} [{unit.Bay.Name}]");
        }
        else
        {
            throw new Exception("Unit not found.");
        }
        ConsoleTools.Pause("Unit shown.");
    }
    #endregion
}