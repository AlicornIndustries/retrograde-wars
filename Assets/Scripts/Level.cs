using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    // Define the tile setup in here (ideally later loading from a file), then generate stuff to match?

    public int cols;
    public int rows;

    public GameManager gameManager;
    public GameObject GrassTilePrefab;
    public GameObject[,] tileObjects;
    public Tile[,] tiles;

    // For testing
    public GameObject tankPrefab;
    public GameObject HQPrefab;

    private void Start()
    {
        tileObjects = new GameObject[cols, rows];
        tiles = new Tile[cols, rows];

        for(int i=0; i<cols; i++)
        {
            for(int j=0; j<rows; j++)
            {
                tileObjects[i, j] = Instantiate(GrassTilePrefab, new Vector3(i,0,j), Quaternion.identity, transform);
                tiles[i, j] = tileObjects[i, j].GetComponent<Tile>();
                tiles[i,j].gridX = i;
                tiles[i,j].gridZ = j;
            }
        }

        SetupTesting();
    }

    private void SetupTesting()
    {
        // Improve this to use this kind of syntax:
        //         BadGuy bg1 = new BadGuy("Harvey", 50);
        // from https://unity3d.com/learn/tutorials/modules/intermediate/scripting/lists-and-dictionaries

        // Make test units
        GameObject testTankObject = Instantiate(tankPrefab);
        Unit testTank = testTankObject.GetComponent<Unit>();
        testTank.level = this;
        testTank.SetOwner(gameManager.currentPlayer);
        testTank.PlaceAt(GridCoordsToTile(1, 0));

        GameObject testTank2Object = Instantiate(tankPrefab);
        Unit testTank2 = testTank2Object.GetComponent<Unit>();
        testTank2.level = this;
        testTank2.SetOwner(gameManager.players[1]);
        testTank2.GetComponent<Renderer>().material.color = Color.red;
        testTank2.PlaceAt(GridCoordsToTile(3, 2));

        GameObject testTank3Object = Instantiate(tankPrefab);
        Unit testTank3 = testTank3Object.GetComponent<Unit>();
        testTank3.level = this;
        testTank3.SetOwner(gameManager.players[1]);
        testTank3.GetComponent<Renderer>().material.color = Color.red;
        testTank3.PlaceAt(GridCoordsToTile(3, 3));

        GameObject HQObject = Instantiate(HQPrefab);
        Unit HQ = HQObject.GetComponent<Unit>();
        HQ.level = this;
        HQ.SetOwner(gameManager.currentPlayer);
        HQ.PlaceAt(GridCoordsToTile(0, 0));
        gameManager.players[0].HQ = HQ;

        GameObject HQObject2 = Instantiate(HQPrefab);
        Unit HQ2 = HQObject2.GetComponent<Unit>();
        HQ2.level = this;
        HQ2.SetOwner(gameManager.players[1]);
        HQ2.GetComponent<Renderer>().material.color = Color.red;
        HQ2.PlaceAt(GridCoordsToTile(4, 2));
        gameManager.players[1].HQ = HQ2;

        // Setup AI
        Player AIPlayer = gameManager.players[1];
        AISoul soul = AIPlayer.soul;
        // Form an Offense group and an HQ group
        List<Unit> testUnits = new List<Unit>();
        testUnits.Add(testTank2);
        testUnits.Add(testTank3);
        soul.MakeControlGroup(ControlGroupTag.Offense, testUnits);

    }

    public Tile GridCoordsToTile(int gridX, int gridZ)
    {
        return tiles[gridX, gridZ];
    }


    // // Code moved to Unit
    //public List<Tile> FindTilesInRange(Tile center, int range)
    //{
    //    // manhattan distance for now
    //    // quick and dirty: loop across all tiles? Find grid coords, then convert to tiles.
    //    // We can improve this later by having preset tile offsets to check (so for range==1, only check those 9 tiles)

    //    List<Tile> tilesInRange = new List<Tile>();

    //    foreach(Tile tile in tiles)
    //    {
    //        if( Mathf.Abs((tile.gridX - center.gridX)) + Mathf.Abs(tile.gridZ - center.gridZ) <= range )
    //        {
    //            tilesInRange.Add(tile);
    //        }
    //    }
    //    return tilesInRange;
    //}
}
