using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public int gridX;
    public int gridZ;

    private Unit unitOnTile; // make this a getter?
    public Unit UnitOnTile
    {
        get { return unitOnTile; }
        set { unitOnTile = value; }
    }

    public void PrintCoords()
    {
        Debug.Log(gridX.ToString() + "," + gridZ.ToString());
    }

}
