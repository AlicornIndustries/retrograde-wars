using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISoul : MonoBehaviour {
    // We want a Utility AI sort of thing.
    // Strategic level: what do we want to do? Aggress, defend, etc.
    // Tactical level: how do we accomplish those goals? Give orders to specific units.

    // Considerations: destroy enemy units, preserve own units, occupy good terrain
    // Stored in a dictionary?

    // Each Soul has a Personality Profile, made up of one or more (summed) Personality Types.
    // Personality Types are essentially numerical modifiers to Considerations.
    // An AI with an Aggressive Personality Type has a naturally high value for the Destroy Enemy Units consideration.
    // An AI with a Vengeful type increases that consideration whenever a Own Unit Destroyed event occurs.

    // Events: things that happen in the game, often affecting Considerations.
    // Own Unit Destroyed, HQ Took Damage, Enemy Approached Objective
    // Enemy Nearby, etc.

    enum Consideration {AttackEnemy, PreserveOwnUnits, OccupyGoodTerrain };
    Dictionary<Consideration, float> considerations = new Dictionary<Consideration, float>();

    public Player player;
    public List<ControlGroup> cgs;

    private void Start()
    {
        considerations.Add(Consideration.AttackEnemy, 100);
        considerations.Add(Consideration.PreserveOwnUnits, 20);
        considerations.Add(Consideration.OccupyGoodTerrain, 50);
    }

    public void Think()
    {
        DecideCourse(); // rename this Plan(), then EnactPlan? Or DecideGoals, Plan, EnactPlan?
        // EnactCourse
    }

    // Make a decision: take the consideration with the highest value, then choose an action based on that.
    // We may add a random modifier to this, bigger if the Soul has the Random personality type.

    private void DecideCourse()
    {
        // Determine most pressing consideration
        // If AttackEnemy, find own unit closest to an enemy and shoot/move to shoot it
        // Later, we improve this so it figures out if it should attack with that unit (are we sending infantry against a Gattling?)
        // Weigh the options to determine which unit to use: weight how much damage the unit would cause against Consideration.PreserveOwnUnits, for example.

        // Find consideration with highest value
        Consideration primaryConsideration = Consideration.AttackEnemy;
        foreach(KeyValuePair<Consideration,float> pair in considerations)
        {
            if(pair.Value > considerations[primaryConsideration])
            {
                primaryConsideration = pair.Key;
            }
        }
        // Now tumble down an ugly if tree
        if(primaryConsideration == Consideration.AttackEnemy)
        {
            Debug.Log("Primary consideration: AttackEnemy");
            // For now, go through our list of units, see if it can shoot an enemy; if so, shoot.
            // Keep going until we run out of units? Go through a number of units determined by how high our Consideration value is? (picking the ones with the highest Reward value to perfom this action, e.g. choose ones that deal most damage)


            /*
            List<Unit> unitsAbleToAct = new List<Unit>(player.unitsAbleToAct); // Since a unit will stop being able to act if it shoots, we need to make a copy of the list that doesn't change to properly iterate over it.
            foreach (Unit unit in unitsAbleToAct)
            {
                if (unit.hasEnemyInShootRange)
                {
                    unit.TryToShoot(unit.EnemiesInShootRange[Random.Range(0, unit.EnemiesInShootRange.Count)]);
                }
                // else, move to nearest enemy?
            }
            */

            // Tell a control group with a relevant CGTag to attack.

        }
        else if(primaryConsideration == Consideration.PreserveOwnUnits)
        {
            Debug.Log("Primary consideration: PreserveOwnUnits");
        }
        else if(primaryConsideration == Consideration.OccupyGoodTerrain)
        {
            Debug.Log("Primary consideration: OccupyGoodTerrain");
        }


    }

    public void MakeControlGroup(ControlGroupTag cgTag, List<Unit> units)
    {
        //ControlGroup newGroup = new ControlGroup();
        ControlGroup newGroup = (ControlGroup)ScriptableObject.CreateInstance(typeof(ControlGroup));
        newGroup.AddUnits(units);
        newGroup.AddTag(cgTag);
        cgs.Add(newGroup);
    }

    public Unit FindUnitOfType(UnitType unitType, bool stealFromOtherCG)
    {
        if(!stealFromOtherCG)
        {
            foreach(Unit unit in player.units)
            {
                if(unit.HasUnitType(unitType) && !unit.IsInControlGroup())
                {
                    return unit;
                }
            }
            // No matching unit found
            return null;
        }
        else
        {
            foreach (Unit unit in player.units)
            {
                if (unit.HasUnitType(unitType))
                {
                    return unit;
                }
            }
            // No matching unit found
            return null;
        }
    }

    public List<Unit> FindUnitsOfType(UnitType unitType, int num, bool stealFromOtherCG)
    {
        // If can't find enough units, it'll return all it could
        int foundNum = 0;
        List<Unit> foundUnits = new List<Unit>();

        if(!stealFromOtherCG)
        {
            foreach(Unit unit in player.units)
            {
                if(unit.HasUnitType(unitType) && !unit.IsInControlGroup())
                {
                    foundUnits.Add(unit);
                    foundNum++;
                    if(foundNum >= num)
                    {
                        return foundUnits;
                    }
                }
            }
            return foundUnits;
        }
        else
        {
            foreach (Unit unit in player.units)
            {
                if (unit.HasUnitType(unitType))
                {
                    foundUnits.Add(unit);
                    foundNum++;
                    if (foundNum >= num)
                    {
                        return foundUnits;
                    }
                }
            }
            return foundUnits;
        }
    }
}
