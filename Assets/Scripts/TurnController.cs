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
    public GameObject defaultUnit; // The default unit selected when game starts

    // Turn Control Var
    public bool playerTurn = true; // Check if player turn
    public bool enemyTurn = false; // Check if enemy turn
    public Button changeTurn; // Pretty self explanatory
    public Text turn;

    // Selected Unit Var
    public Text unit; // UI Text with unit name
    public Text atkStrength; // UI Text with unit atk
    public Text defStrength; // UI Text with unit def
    public float unitAtk; 
    public float unitDef;
    public string unitType;
    public float unitDistance;
    public float maximumMoves;

    // Use this for initialization
    void Start()
    {
        unitAtk = CapsuleUnit.atk;
        unitDef = CapsuleUnit.def;
        unitType = CapsuleUnit.playerClass;
    }

    // Update is called once per frame
    void Update()
    {
        SelectUnit();
        MoveUnit();
        UpdateSelectedUnitUI();
        changeTurnUI();
    }

    void changeTurnUI()
    {
        if (playerTurn == true)
        {
            turn.text = ("Player Turn");
        }
        else if (enemyTurn == true)
        {
            turn.text = ("Enemy Turn");
        }
    }

    void UpdateSelectedUnitUI()
    {
        if (unitSelected == true)
        {
            // TODO SELECTED UNIT CHARACTERISTICS CHANGE IN UI
        }
        else
        {
            unit.text = (unitType);
            atkStrength.text = ("ATK: " + unitAtk);
            defStrength.text = ("DEF: " + unitDef);
        }
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
                        maximumMoves = CapsuleUnit.maxMoves;
                        // TODO SELECTED UNIT CHARACTERISTICS
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
                if (maximumMoves >= 1)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Ray mousePos = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hitResults;
                        if (Physics.Raycast(mousePos, out hitResults))
                        {
                            GameObject hitObject = hitResults.collider.gameObject;
                            unitDistance = Vector3.Distance(selectedUnit.transform.position,
                                                            hitObject.transform.position);
                            if (hitObject.tag == "Tile")
                            {
                                if (unitDistance <= 4)
                                {
                                    maximumMoves -= 1;
                                    // TODO HIGHLIGHT IN RANGE TILES
                                    selectedUnit.transform.position = hitObject.transform.position;
                                    unitSelected = false; // Tells the game that there is no longer a unit selected
                                }
                            }
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
        }
    }
}