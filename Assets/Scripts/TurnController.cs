using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TurnController : MonoBehaviour
{

    public EventSystem LevelEventSystem;

    // Unit Control Var
    public GameObject selectedUnit; // Unit selected to move
    public bool unitSelected = false; // Check if unit is currently selected

    // Turn Control Var
    public bool playerTurn = true; // Check if player turn
    public bool enemyTurn = false; // Check if enemy turn

    public Button changeTurn;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SelectUnit();
        MoveUnit();
    }

    void SelectUnit() // Selects a unit to move 
    {
        if (playerTurn == true)
        {
            // Check for user click on enemy
            if (Input.GetMouseButtonDown(0))
            {
                Ray mousePos = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitResults;
                if (Physics.Raycast(mousePos, out hitResults))
                {
                    GameObject hitObject = hitResults.collider.gameObject;

                    if (hitObject.tag == "PlayerUnit")
                    {
                        Debug.Log("Player Unit Selected");
                        selectedUnit = hitObject;
                        unitSelected = true; // Tells the game that there is currently a unit selected
                    }
                }
            }
        }
    }

    void MoveUnit() // Moves the currently selected unit
    {
        if (playerTurn == true)
        {
            if (unitSelected == true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray mousePos = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hitResults;
                    if (Physics.Raycast(mousePos, out hitResults))
                    {
                        GameObject hitObject = hitResults.collider.gameObject;

                        if (hitObject.tag == "Tile")
                        {
                            selectedUnit.transform.position = hitObject.transform.position;
                            unitSelected = false; // Tells the game that there is no longer a unit selected
                        }
                    }
                }
            }
        }
    }

    public void EndTurn() // Start enemy turn
    {
        if (playerTurn == true)
        {
            playerTurn = false;
            enemyTurn = true;
            Debug.Log("Enemy Phase");
        }
    }
}
