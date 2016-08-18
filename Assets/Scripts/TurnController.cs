using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TurnController : MonoBehaviour
{

    public EventSystem LevelEventSystem;

    AudioManager audioManager;

    // Unit Control Var
    public GameObject selectedUnit; // Unit selected to move
    public bool unitSelected = false; // Check if unit is currently selected

    // Turn Control Var
    public bool playerTurn = true; // Check if player turn
    public bool enemyTurn = false; // Check if enemy turn
    public bool playerAttack = false;
    public bool enemyAttack = false;
    public Button changeTurn; // Pretty self explanatory
    public Text turn;

    // Selected Unit Var
    public Text unit; // UI Text with unit name
    public Text atkStrength; // UI Text with unit atk
    public Text defStrength; // UI Text with unit def
    public float unitDistance; // Distance to the tile
    public Text health; // UI Text with unit health
    public Text movesLeft;
    public Text turnNumberUI;
    public float turnNumber = 1;

    // Target Unit Var
    public Text targetUnit; // UI Text with unit name
    public Text targetAtkStrength; // UI Text with unit atk
    public Text targetDefStrength; // UI Text with unit def
    public Text targetHealth; // UI Text with unit health

    // Battle Var
    public float AttackerTempAtk;
    public float DefenderTempAtk;
    public int playerUnits = 5;
    public int enemyUnits = 5;
    public GameObject battleExplosion;

    // Blue Team Audio
    public AudioClip blueTeamSortie;
    public AudioClip blueTeamBattle;
    public AudioClip blueTeamStruggle;

    // Red Team Audio
    public AudioClip redTeamSortie;
    public AudioClip redTeamBattle;
    public AudioClip redTeamStruggle;


    // Use this for initialization
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        InitialAudio();
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
        BattleMusic();
        turnNumberUI.text = "Turn: " + turnNumber;
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
            unit.text = (selectedUnit.GetComponent<CapsuleUnit>().playerClass);
            atkStrength.text = ("ATK: " + selectedUnit.GetComponent<CapsuleUnit>().atk);
            defStrength.text = ("DEF: " + selectedUnit.GetComponent<CapsuleUnit>().def);
            health.text = ("Health: " + selectedUnit.GetComponent<CapsuleUnit>().health);
            movesLeft.text = ("Moves left: " + selectedUnit.GetComponent<CapsuleUnit>().maxMoves);
    }

    public void hitObjectUI(GameObject hitObject)
    {
        targetUnit.text = (hitObject.GetComponent<CapsuleUnit>().playerClass);
        targetAtkStrength.text = ("ATK: " + hitObject.GetComponent<CapsuleUnit>().atk);
        targetDefStrength.text = ("DEF: " + hitObject.GetComponent<CapsuleUnit>().def);
        targetHealth.text = ("Health: " + hitObject.GetComponent<CapsuleUnit>().health);
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
                                hitObjectUI(hitObject);
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
                                hitObjectUI(hitObject);
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
        AttackerTempAtk = selectedUnit.GetComponent<CapsuleUnit>().atk - hitObject.GetComponent<CapsuleUnit>().def / 2;
        hitObject.GetComponent<CapsuleUnit>().health -= AttackerTempAtk;
        Instantiate(battleExplosion, hitObject.transform.position, Quaternion.identity);
        if (selectedUnit.gameObject.tag == "PlayerUnit")
        {
            playerAttack = true;
        }
        else if (selectedUnit.gameObject.tag == "EnemyUnit")
        {
            enemyAttack = true;
        }
        DefenderTempAtk = hitObject.GetComponent<CapsuleUnit>().atk - selectedUnit.GetComponent<CapsuleUnit>().def;
        selectedUnit.GetComponent<CapsuleUnit>().health -= AttackerTempAtk;
        Instantiate(battleExplosion, selectedUnit.transform.position, Quaternion.identity);
        hitObjectUI(hitObject);
    }

    public void EndTurn() // Start enemy turn
    {
        if (playerTurn == true)
        {
            playerTurn = false;
            enemyTurn = true;
            unitSelected = false;
            audioManager.stopAudio();
            audioManager.playAudio(blueTeamSortie);
            turnNumber += 1;
            playerAttack = false;
        }
        else if (enemyTurn == true)
        {
            playerTurn = true;
            enemyTurn = false;
            unitSelected = false;
            audioManager.stopAudio();
            audioManager.playAudio(redTeamSortie);
            turnNumber += 1;
            enemyAttack = false;
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

    public void InitialAudio()
    {
        if(turnNumber == 1)
        {
            audioManager.playAudio(redTeamSortie);
        }
    }

    public void BattleMusic()
    {
        if (playerTurn == true && playerAttack == true)
        {
            audioManager.stopAudio();
            audioManager.playAudio(redTeamBattle);
            playerAttack = false;
        }
        else if (enemyTurn == true && enemyAttack == true)
        {
            audioManager.stopAudio();
            audioManager.playAudio(blueTeamBattle);
            enemyAttack = false;
        }
    }
}