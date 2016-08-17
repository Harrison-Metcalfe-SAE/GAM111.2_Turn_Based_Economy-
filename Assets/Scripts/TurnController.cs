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
    public Button changeTurn; // Pretty self explanatory
    public Text turn;

    // Selected Unit Var
    public Text unit; // UI Text with unit name
    public Text atkStrength; // UI Text with unit atk
    public Text defStrength; // UI Text with unit def
    public float unitDistance; // Distance to the tile
    public Text health; // UI Text with unit health

    // Battle Var
    public float AttackerTempAtk;
    public float DefenderTempAtk;
    public int playerUnits = 5;
    public int enemyUnits = 5;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SelectUnitRed();
        SelectUnitBlue();
        MoveUnitRed();
        MoveUnitBlue();
        UpdateSelectedUnitUI();
        ChangeTurnUI();
        LoseGame();
    }

    void ChangeTurnUI()
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
            unit.text = (selectedUnit.GetComponent<CapsuleUnit>().playerClass);
            atkStrength.text = ("ATK: " + selectedUnit.GetComponent<CapsuleUnit>().atk);
            defStrength.text = ("DEF: " + selectedUnit.GetComponent<CapsuleUnit>().def);
            health.text = ("Health: " + selectedUnit.GetComponent<CapsuleUnit>().health);
        }
    }

    void SelectUnitRed() // Selects a unit to move 
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
                        // maximumMoves = CapsuleUnit.maxMoves;
                        Debug.Log("Red Player Unit Selected");
                        selectedUnit = hitObject;
                        unitSelected = true; // Tells the game that there is currently a unit selected
                    }
                }
            }
        }
    }

    void SelectUnitBlue() // Selects a unit to move 
    {
        if (enemyTurn == true)
        {
            // Check for user click on enemy
            if (Input.GetMouseButtonDown(0))
            {
                Ray mousePos = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitResults;
                if (Physics.Raycast(mousePos, out hitResults))
                {
                    GameObject hitObject = hitResults.collider.gameObject;

                    if (hitObject.tag == "EnemyUnit")
                    {
                        // maximumMoves = CapsuleUnit.maxMoves;
                        Debug.Log("Blue Player Unit Selected");
                        selectedUnit = hitObject;
                        unitSelected = true; // Tells the game that there is currently a unit selected
                    }
                }
            }
        }
    }

    void MoveUnitRed() // Moves the currently selected unit
    {
        if (playerTurn == true)
        {
            if (unitSelected == true)
            {
                if (selectedUnit.GetComponent<CapsuleUnit>().maxMoves >= 1)
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
                                if (unitDistance <= selectedUnit.GetComponent<CapsuleUnit>().travelDist)
                                {
                                    selectedUnit.GetComponent<CapsuleUnit>().maxMoves -= 1;
                                    selectedUnit.transform.position = hitObject.transform.position;
                                    unitSelected = false; // Tells the game that there is no longer a unit selected
                                }
                            }
                            else if (hitObject.tag == "EnemyUnit")
                            {
                                if (unitDistance <= selectedUnit.GetComponent<CapsuleUnit>().travelDist)
                                {
                                    Battle(hitObject);
                                    selectedUnit.GetComponent<CapsuleUnit>().maxMoves -= 1;
                                    unitSelected = false; // Tells the game that there is no longer a unit selected
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    void MoveUnitBlue() // Moves the currently selected unit
    {
        if (enemyTurn == true)
        {
            if (unitSelected == true)
            {
                if (selectedUnit.GetComponent<CapsuleUnit>().maxMoves >= 1)
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
                                if (unitDistance <= selectedUnit.GetComponent<CapsuleUnit>().travelDist)
                                {
                                    selectedUnit.GetComponent<CapsuleUnit>().maxMoves -= 1;
                                    selectedUnit.transform.position = hitObject.transform.position;
                                    unitSelected = false; // Tells the game that there is no longer a unit selected
                                }
                            }
                            else if (hitObject.tag == "PlayerUnit")
                            {
                                if (unitDistance <= selectedUnit.GetComponent<CapsuleUnit>().travelDist)
                                {
                                    Battle(hitObject);
                                    selectedUnit.GetComponent<CapsuleUnit>().maxMoves -= 1;
                                    unitSelected = false; // Tells the game that there is no longer a unit selected
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void Battle(GameObject hitObject)
    {
        AttackerTempAtk = selectedUnit.GetComponent<CapsuleUnit>().atk - hitObject.GetComponent<CapsuleUnit>().def;
        hitObject.GetComponent<CapsuleUnit>().health -= AttackerTempAtk;
        DefenderTempAtk = hitObject.GetComponent<CapsuleUnit>().atk - selectedUnit.GetComponent<CapsuleUnit>().def;
        selectedUnit.GetComponent<CapsuleUnit>().health -= AttackerTempAtk;
    }

    public void EndTurn() // Start enemy turn
    {
        if (playerTurn == true)
        {
            playerTurn = false;
            enemyTurn = true;
            unitSelected = false;
        }
        else if (enemyTurn == true)
        {
            playerTurn = true;
            enemyTurn = false;
            unitSelected = false;
        }
        foreach (CapsuleUnit unit in GameObject.FindObjectsOfType<CapsuleUnit>())
        {
            unit.maxMoves = unit.maxMaxMoves;
        }
    }

    public void LoseGame()
    {
        if (playerUnits <= 0)
        {

        }
        else if (enemyUnits <= 0)
        {

        }
        else if (playerUnits <= 0 && enemyUnits <= 0)
        {

        }
    }
}