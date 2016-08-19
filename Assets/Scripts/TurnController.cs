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
    public bool playerAttack = false; // Has the player attacked this turn?
    public bool enemyAttack = false; // Has the other player attack this turn?
    public Button changeTurn; // Pretty self explanatory
    public Button quitGame; // Bail on the game
    public Button resetButton; // Runback
    public Text turn; // Whose turn is it?
    public Text winner; // Who won?

    // Selected Unit Var
    public Text unit; // UI Text with unit name
    public Text atkStrength; // UI Text with unit atk
    public Text defStrength; // UI Text with unit def
    public float unitDistance; // Distance to the tile
    public Text health; // UI Text with unit health
    public Text movesLeft; // How many times the unit can move
    public Text turnNumberUI; // What turn is it?
    public float turnNumber = 1; // The turn number

    // Target Unit Var
    public Text targetUnit; // UI Text with unit name
    public Text targetAtkStrength; // UI Text with unit atk
    public Text targetDefStrength; // UI Text with unit def
    public Text targetHealth; // UI Text with unit health

    // Battle Var
    public float AttackerTempAtk; // Temp attack taken from the attacker
    public float DefenderTempAtk; // Temp attack taken from the defender
    public int playerUnits = 1; // How many units does the player have?
    public int enemyUnits = 1; // How many units does the enemy have?
    public GameObject battleExplosion; // Battle effects
    public bool battleMusicPlay = false; // Should the battle music be playing

    // Blue Team Audio
    public AudioClip blueTeamSortie; // Pre-battle music
    public AudioClip blueTeamBattle; // Battle music
    public AudioClip blueTeamStruggle; // Less than 3 unit music

    // Red Team Audio
    public AudioClip redTeamSortie; // Pre-battle music
    public AudioClip redTeamBattle; // Battle music
    public AudioClip redTeamStruggle; // Less than 3 unit music

    // Use this for initialization
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        InitialAudio();
    }

    // Update is called once per frame
    void Update()
    {
        SelectUnitRed(); // Is the red player selecting a unit?
        SelectUnitBlue(); // Is the blue player selecting a unit?
        MoveUnitRed(); // Move units red
        MoveUnitBlue(); // Move units blue
        UpdateSelectedUnitUI(); // Selected unit stats moved to UI
        ChangeTurnUI(); // Changes the turn as displayed in the UI
        BattleMusic(); // Plays battle music
        Winner(); // Displays winner based on lose condition
        turnNumberUI.text = "Turn: " + turnNumber;
    }

    void ChangeTurnUI() // Change turn UI
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

    void UpdateSelectedUnitUI() // Update UI
    {
            unit.text = (selectedUnit.GetComponent<CapsuleUnit>().playerClass);
            atkStrength.text = ("ATK: " + selectedUnit.GetComponent<CapsuleUnit>().atk);
            defStrength.text = ("DEF: " + selectedUnit.GetComponent<CapsuleUnit>().def);
            health.text = ("Health: " + selectedUnit.GetComponent<CapsuleUnit>().health);
            movesLeft.text = ("Moves left: " + selectedUnit.GetComponent<CapsuleUnit>().maxMoves);
    }

    public void hitObjectUI(GameObject hitObject) // Update UI
    {
        targetUnit.text = (hitObject.GetComponent<CapsuleUnit>().playerClass);
        targetAtkStrength.text = ("ATK: " + hitObject.GetComponent<CapsuleUnit>().atk);
        targetDefStrength.text = ("DEF: " + hitObject.GetComponent<CapsuleUnit>().def);
        targetHealth.text = ("Health: " + hitObject.GetComponent<CapsuleUnit>().health);
    }

    void SelectUnitRed() // Selects a unit to move if it's the red player's turn
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
                        selectedUnit = hitObject;
                        unitSelected = true; // Tells the game that there is currently a unit selected
                    }
                }
            }
        }
    }

    void SelectUnitBlue() // Selects a unit to move if it's the blue player's turn
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
                        selectedUnit = hitObject;
                        unitSelected = true; // Tells the game that there is currently a unit selected
                    }
                }
            }
        }
    }

    void MoveUnitRed() // Moves the currently selected unit if it's the red player's turn
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
                                    if (unitDistance < 1.5)
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
    }

    void MoveUnitBlue() // Moves the currently selected unit if it's the blue player's turn
    {
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
                                        if (unitDistance < 1.5)
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
        }
    }

    public void Battle(GameObject hitObject) // Battle function
    {
        AttackerTempAtk = selectedUnit.GetComponent<CapsuleUnit>().atk - hitObject.GetComponent<CapsuleUnit>().def / 2;  // Defending unit has half defence
        hitObject.GetComponent<CapsuleUnit>().health -= AttackerTempAtk;
        Instantiate(battleExplosion, hitObject.transform.position, Quaternion.identity); // Explosion effect when battle happens
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

    public void EndTurn() // Start enemy turn, reset variables and change music
    {
        if (playerTurn == true)
        {
            playerTurn = false;
            enemyTurn = true;
            unitSelected = false;
            audioManager.stopAudio();
            if (playerUnits <= 3)
            {
                audioManager.playAudio(blueTeamStruggle);
            }
            else if (playerUnits > 3)
            {
                audioManager.playAudio(blueTeamSortie);
            }
            turnNumber += 1;
            playerAttack = false;
            battleMusicPlay = false;
        }
        else if (enemyTurn == true)
        {
            playerTurn = true;
            enemyTurn = false;
            unitSelected = false;
            audioManager.stopAudio();
            if (playerUnits <= 3)
            {
                audioManager.playAudio(redTeamStruggle);
            }
            else if (playerUnits > 3)
            {
                audioManager.playAudio(redTeamSortie);
            }
            turnNumber += 1;
            enemyAttack = false;
            battleMusicPlay = false;
        }
        foreach (CapsuleUnit unit in GameObject.FindObjectsOfType<CapsuleUnit>())
        {
            unit.maxMoves = unit.maxMaxMoves;
        }
    }

    public void InitialAudio() // Plays the Red Team Sortie music when game starts
    {
        if(turnNumber == 1)
        {
            audioManager.playAudio(redTeamSortie);
        }
    }

    public void BattleMusic() // Plays battle music after unit attacks
    {
        if (playerTurn == true && playerAttack == true && battleMusicPlay == false && playerUnits >= 3)
        {
            audioManager.stopAudio();
            audioManager.playAudio(redTeamBattle);
            playerAttack = false;
            battleMusicPlay = true;
        }
        else if (enemyTurn == true && enemyAttack == true && battleMusicPlay == false && enemyUnits >= 3)
        {
            audioManager.stopAudio();
            audioManager.playAudio(blueTeamBattle);
            enemyAttack = false;
            battleMusicPlay = true;
        }
    }

    public void QuitGame() // Exactly what it says on the tin
    {
        Application.Quit();
    }

    public void Winner()
    {
        if (playerUnits <= 0)
        {
            winner.text = "Blue Wins!";
        }
        if (enemyUnits <= 0)
        {
            winner.text = "Red Wins!";
        }
    }

    public void Reset() // Also exactly what it says on the tin
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}