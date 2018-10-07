using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum ControlGroupType {Armor, Airstrike, HQ }
public enum ControlGroupTag {Offense, Defense, Air, DirectFire, IndirectFire }
public enum ControlGroupOrders {AttackHQ, DefendHQ, Rampage, Retreat, HoldPosition}

public class ControlGroup: ScriptableObject {
    public List<Unit> units = new List<Unit>();
    public ControlGroupTag cgTag;
    public Player owner;

    public bool HasCGTag(ControlGroupTag inputcgTag)
    {
        if(inputcgTag == cgTag)
        {
            return true;
        }
        return false;
    }


    public void ReceiveOrder(ControlGroupOrders order)
    {
        if(order == ControlGroupOrders.AttackHQ)
        {

        }
        else if(order == ControlGroupOrders.DefendHQ)
        {

        }
        // Rampage: find nearest enemy and engage
    }

    public void AddUnits(List<Unit> newUnits)
    {
        units.AddRange(newUnits);
    }

    public void AddTag(ControlGroupTag newcgTag)
    {
        // For now, only support having one tag
        cgTag = newcgTag;
    }
}
