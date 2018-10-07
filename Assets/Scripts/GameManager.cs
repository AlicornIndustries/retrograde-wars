using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Camera cam;
    public Level level;

    public List<Player> players;
    public Player currentPlayer;

    private bool waitingForPlayerInput = true;
    private Tile selectedTile;
    private Unit selectedUnit;

    public GameObject reticule;
    public LayerMask tileLayerMask;

    private int turnNumber = 1;

    private void Start()
    {
        currentPlayer = players[0];
    }

    private void Update()
    {
        if(waitingForPlayerInput)
        {
            // Selection
            if(Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayerMask))
                {
                    if (hit.collider.gameObject.CompareTag("Clickable"))
                    {
                        selectedTile = hit.collider.gameObject.GetComponent<Tile>();
                        reticule.transform.position = new Vector3(selectedTile.transform.position.x, selectedTile.transform.position.y + (float)0.1, selectedTile.transform.position.z);
                        if (selectedTile.UnitOnTile != null)
                        {
                            //selectedUnit = selectedTile.UnitOnTile;
                            SelectUnit(selectedTile.UnitOnTile);
                        }
                    }
                }
            }

            // Movement
            if (Input.GetMouseButtonDown(1))
            {
                if (selectedUnit != null)
                {
                    RaycastHit hit;
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayerMask))
                    {
                        if (hit.collider.gameObject.GetComponent<Tile>())
                        {
                            Tile targetTile = hit.collider.gameObject.GetComponent<Tile>();
                            // Move if the tile is clear, try to shoot if there's an enemy
                            if(!targetTile.UnitOnTile)
                            {
                                selectedUnit.TryToMove(targetTile);
                            }
                            else if(targetTile.UnitOnTile.owner.IsEnemyOf(selectedUnit.owner))
                            {
                                selectedUnit.TryToShoot(targetTile.UnitOnTile);
                            }
                            //selectedUnit.PlaceAt(hit.collider.gameObject.GetComponent<Tile>());
                        }
                    }
                }
                CheckIfShouldAdvance();
            }

            // Manually end turn
            if (Input.GetKeyDown(KeyCode.Return))
            {
                AdvanceTurn();
            }
        }
    }

    private void SelectUnit(Unit unit)
    {
        selectedUnit = unit;
        selectedUnit.OnSelect();
    }

    private void CheckIfShouldAdvance()
    {
        if(!currentPlayer.StillHasActions())
        {
            AdvanceTurn();
        }
    }

    private void AdvanceTurn()
    {
        int currentPlayerIndex = players.IndexOf(currentPlayer);
        currentPlayerIndex++;
        if (currentPlayerIndex >= players.Count)
        {
            currentPlayerIndex = 0;
        }
        currentPlayer = players[currentPlayerIndex];

        turnNumber++;
        Debug.Log("Turn " + turnNumber.ToString());

        currentPlayer.RefreshAllUnits();
        if(currentPlayer.isAI)
        {
            waitingForPlayerInput = false;
            ManageAITurn(currentPlayer);
        }
        else
        {
            waitingForPlayerInput = true;
        }
    }

    private void ManageAITurn(Player player)
    {
        player.soul.Think();
        AdvanceTurn();
    }
}
