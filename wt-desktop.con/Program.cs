using Microsoft.EntityFrameworkCore;
using wt_desktop.ef;
using wt_desktop.ef.Entity;

namespace wt_desktop.con;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string reference  = "teehee";
            ConsoleTools.Display("Database connection established.", "debug");

            ConsoleTools.Pause(WtContext.Instance.Database.CanConnect() ? "Database connection successful." : "Database connection failed.");

            Bay bay = GenerateBay(reference);
            bay = WtContext.Instance.Bay.FirstOrDefault(u => u.Name == reference) ?? bay;
            SearchBay(bay.Id);

            Unit unit = GenerateUnit(reference, bay);
            unit = WtContext.Instance.Unit.FirstOrDefault(u => u.Name == reference) ?? unit;
            SearchUnit(unit.Id);
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
        Bay testBay = new Bay
        {
            Name = reference,
            Location = "Test Location"
        };

        WtContext.Instance.Bay.Add(testBay);
        WtContext.Instance.SaveChanges();

        ConsoleTools.Pause("Bay added.");

        return testBay;
    }

    static Unit GenerateUnit(string reference, Bay bay)
    {
        Unit testUnit = new Unit
        {
            Name = reference,
            IsStarted = true,
            Status = Unit.EUnitStatus.OK,
            Bay = bay,
            UnitUsage = null
        };

        WtContext.Instance.Unit.Add(testUnit);
        WtContext.Instance.SaveChanges();

        ConsoleTools.Pause("Unit added.");

        return testUnit;
    }
    #endregion

    #region display
    static void SearchBay(int id)
    {
        Bay? bay = WtContext.Instance.Bay.FirstOrDefault(u => u.Id == id);

        if (bay != null)
        {
            ConsoleTools.Display($"Bay found: {bay.Id} ({bay.Units(WtContext.Instance).ToList().Count} units)");
        }
        else
        {
            throw new Exception("Bay not found.");
        }
        ConsoleTools.Pause("Bay shown.");
    }

    static void SearchUnit(int id)
    {
        Unit? unit = WtContext.Instance.Unit.FirstOrDefault(u => u.Id == id);

        if (unit != null)
        {
            ConsoleTools.Display($"Unit found: {unit.Id}");
            if (unit.Bay != null)
            {
                ConsoleTools.Display($"Unit's bay: {unit.Bay.Id}");
            }
            else
            {
                ConsoleTools.Display("Unit's bay not found.");
            }
        }
        else
        {
            throw new Exception("Unit not found.");
        }
        ConsoleTools.Pause("Unit shown.");
    }
    #endregion
}