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
            WtContext context = new WtContext();
            ConsoleTools.Display("Database connection established.", "debug");

            ConsoleTools.Pause(context.Database.CanConnect() ? "Database connection successful." : "Database connection failed.");

            Bay  bay  = GenerateBay(reference, context);
            Unit unit = GenerateUnit(reference, bay, context);

            context.SaveChanges();

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
    static Bay GenerateBay(string reference, WtContext context)
    {
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

    static Unit GenerateUnit(string reference, Bay bay, WtContext context)
    {
        Unit testUnit = new Unit
        {
            Name = reference,
            IsStarted = true,
            Status = Unit.EUnitStatus.OK,
            Bay = bay,
            UnitUsage = null
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
            ConsoleTools.Display($"Bay found: {bay.Id} ({bay.Units(context).ToList().Count} units)");
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

        Unit? unit = context.Unit.Include(unit => unit.Bay).FirstOrDefault(u => u.Name == reference);

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