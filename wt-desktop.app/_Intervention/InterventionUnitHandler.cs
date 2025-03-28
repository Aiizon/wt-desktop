using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using wt_desktop.ef;
using wt_desktop.ef.Entity;

namespace wt_desktop.app;

public static class InterventionUnitHandler
{
    public static void HandleSave
    (
        Intervention       intervention,
        IEnumerable<Bay?>  bays,
        IEnumerable<Unit?> units
    ) {
        HashSet<int> selectedUnits = new();

        foreach (var unit in units)
        {
            selectedUnits.Add(unit!.Id);
        }
        
        foreach (var bay in bays)
        {
            foreach (var unit in bay!.Units())
            {
                selectedUnits.Add(unit.Id);
            }
        }

        var existingUnits = WtContext.Instance.UnitIntervention
            .Where(ui => ui.InterventionId == intervention.Id);
        
        var existingUnitIds = existingUnits
            .Select(ui => ui.UnitId)
            .ToHashSet();
        
        var toAdd    = selectedUnits.Except(existingUnitIds);
        var toRemove = existingUnitIds.Except(selectedUnits);
        
        foreach (var unitId in toAdd)
        {
            WtContext.Instance.UnitIntervention.Add(new UnitIntervention
            {
                InterventionId = intervention.Id,
                UnitId         = unitId
            });
        }
        
        foreach (var unitId in toRemove)
        {
            var unitIntervention = existingUnits
                .FirstOrDefault(ui => ui.UnitId == unitId);
            
            if (unitIntervention != null)
            {
                WtContext.Instance.UnitIntervention.Remove(unitIntervention);
            }
        }
        
        WtContext.Instance.SaveChanges();
    }
    
    public static (ObservableCollection<Bay?>, ObservableCollection<Unit?>) HandleReset
    (
        Intervention intervention
    ) {
        // Clone la liste d'unités originelle
        var interventionUnits = new HashSet<Unit?>();
        if (intervention.Id != 0)
        {
            interventionUnits = intervention.Units.ToHashSet();
        }
        
        // Trouve les baies ayant toutes leurs unités associées à l'intervention
        // & retire leurs unités de la liste
        var selectedBays = new HashSet<Bay?>();
        var bays         = WtContext.Instance.Bay.ToList();
        foreach (var bay in bays)
        {
            if (bay.Units().Any() && 
                bay.Units().All(unit => interventionUnits.Contains(unit)))
            {
                selectedBays.Add(bay);
                
                foreach (var unit in bay.Units())
                {
                    interventionUnits.Remove(unit);
                }
            }
        }

        // Renvoie les liste d'unités & baies filtrées
        return (new(selectedBays), new(interventionUnits));
    }
}