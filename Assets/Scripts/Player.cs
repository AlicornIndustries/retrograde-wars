using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public bool isAI;
    public AISoul soul; // null if not AI.

    public List<Unit> units;
    public List<Unit> unitsAbleToAct;

    public Unit HQ;

    public void AddUnit(Unit unit)
    {
        units.Add(unit);
        if(unit.canAct)
        {
            unitsAbleToAct.Add(unit);
        }
    }

    public void RemoveUnit(Unit unit)
    {
        units.Remove(unit);
        if(unitsAbleToAct.Contains(unit))
        {
            unitsAbleToAct.Remove(unit);
        }
    }

    public void RecordUnitCantAct(Unit unit)
    {
        if(unitsAbleToAct.Contains(unit))
        {
            unitsAbleToAct.Remove(unit);
        }
    }

    public void RefreshAllUnits()
    {
        // Resets canMove, canShoot, and so forth
        foreach (Unit unit in units)
        {
            if(unit.Refresh() && !unitsAbleToAct.Contains(unit))
            {
                unitsAbleToAct.Add(unit);
            }
        }
    }

    public bool IsEnemyOf(Player other)
    {
        // TODO: expand with proper ally code
        if(other != this)
        {
            return true;
        }
        return false;
    }

    public bool StillHasActions()
    {
        // Returns true if the player still has stuff they can do
        // like moving units or using abilities
        if(unitsAbleToAct.Count > 0)
        {
            return true;
        }
        return false;
    }

}
