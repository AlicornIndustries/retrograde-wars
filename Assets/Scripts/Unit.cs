using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType { Infantry, Tank, HQ}

public class Unit : MonoBehaviour {

    public UnitType unitType;
    public int healthMax;
    public int healthCurr;
    public int damage;
    public int armor;
    public int minis; // number of minatures/models in the unit. Hence, number of shots.
    public int shootRange;
    public int moveRange;
    // public float AIValue; // how valuable does the AI think this is

    private Tile tile;
    private int gridX;
    private int gridZ;

    public Player owner;
    public ControlGroup cg;

    public bool canMove;
    public bool canShoot;
    public bool canAct
    {
        get { return (canMove || canShoot); }
    }
    public bool canMoveAfterShooting;
    // public bool canShootAtEnemy or canEngage to tell us if this has an enemy target to fire upon? get { return (canShoot && HasEnemyWithinRange) };

    private List<Tile> tilesInMoveRange;
    private List<Tile> tilesInShootRange;
    public List<Tile> TilesInMoveRange
    {
        get { return tilesInMoveRange; }
    }
    public List<Tile> TilesInShootRange
    {
        get { return tilesInShootRange; }
    }
    public Level level;

    public List<Unit> enemiesInShootRange;
    public List<Unit> EnemiesInShootRange
    {
        get { return enemiesInShootRange; }
    }
    public bool hasEnemyInShootRange
    {
        get
        {
            if(enemiesInShootRange.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private void Start()
    {
        healthCurr = healthMax;
        SetOwner(owner);
    }

    public void SetOwner(Player newOwner)
    {
        if(owner != null)
        {
            owner.RemoveUnit(this);
        }
        owner = newOwner;
        owner.AddUnit(this);
    }

    public bool TryToMove(Tile targetTile)
    {
        if(canMove && IsTileWithinMoveRange(targetTile) && targetTile != level.GridCoordsToTile(gridX,gridZ) && !targetTile.UnitOnTile)
        {
            PlaceAt(targetTile);
            canMove = false;
            TellOwnerAboutCanAct();
            return true;
        }
        return false;
    }

    public bool TryToShoot(Unit targetUnit)
    {
        // if(canShoot && targetUnit.owner != owner && IsTileWithinShootRange(targetUnit.tile))
        if (canShoot && owner.IsEnemyOf(targetUnit.owner) && IsTileWithinShootRange(targetUnit.tile))
        {
            Shoot(targetUnit);
            canShoot = false;
            if(!canMoveAfterShooting)
            {
                canMove = false;
            }
            TellOwnerAboutCanAct();
            return true;
        }
        return false;
    }

    private void TellOwnerAboutCanAct()
    {
        if(!canAct)
        {
            owner.RecordUnitCantAct(this);
        }
    }

    public bool Refresh()
    {
        // Returns a bool in case we add something that prevents units from refreshing
        canMove = true;
        canShoot = true;
        enemiesInShootRange = FindEnemiesInShootRange();
        return true;
    }

    private bool IsTileWithinMoveRange(Tile tile)
    {
        if(tilesInMoveRange.Contains(tile))
        {
            return true;
        }
        return false;
    }

    private bool IsTileWithinShootRange(Tile tile)
    {
        if(tilesInShootRange.Contains(tile))
        {
            return true;
        }
        return false;
    }

    public void PlaceAt(Tile newTile)
    {
        if(tile != null)
        {
            tile.UnitOnTile = null;
        }
        tile = newTile;
        tile.UnitOnTile = this;
        transform.position = tile.transform.position;
        gridX = tile.gridX;
        gridZ = tile.gridZ;
        tilesInMoveRange = FindTilesInRange(tile, moveRange, level);
        tilesInShootRange = FindTilesInRange(tile, shootRange, level);
        enemiesInShootRange = FindEnemiesInShootRange();
    }

    private void Shoot(Unit targetUnit)
    {
        targetUnit.ReceiveAttack(damage, minis);
    }

    private List<Tile> FindTilesInRange(Tile center, int range, Level level)
    {
        // manhattan distance for now
        // quick and dirty: loop across all tiles? Find grid coords, then convert to tiles.
        // We can improve this later by having preset tile offsets to check (so for range==1, only check those 9 tiles)

        List<Tile> tilesInRange = new List<Tile>();

        foreach (Tile tile in level.tiles)
        {
            if (Mathf.Abs((tile.gridX - center.gridX)) + Mathf.Abs(tile.gridZ - center.gridZ) <= range)
            {
                tilesInRange.Add(tile);
            }
        }
        return tilesInRange;
    }

    private List<Unit> FindEnemiesInShootRange()
    {
        List<Unit> shootableEnemies = new List<Unit>();

        foreach(Tile tile in TilesInShootRange)
        {
            Unit unit = tile.UnitOnTile;
            if(unit != null && owner.IsEnemyOf(unit.owner))
            {
                shootableEnemies.Add(unit);
            }
        }

        return shootableEnemies;
    }

    public void OnSelect()
    {
    }

    public void ReceiveAttack(int attackDamage, int numShots)
    {
        healthCurr -= (attackDamage - armor) * numShots;
        if(healthCurr <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Dead!");
    }

    public bool IsInControlGroup()
    {
        if(cg != null)
        {
            return true;
        }
        return false;
    }

    public bool HasUnitType(UnitType queryUnitType)
    {
        if(unitType == queryUnitType)
        {
            return true;
        }
        return false;
    }

}
