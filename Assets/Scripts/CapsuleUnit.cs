using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CapsuleUnit : MonoBehaviour {

    public string playerClass = "Capsule"; // The unit name
    public float atk = 10f; // Attack stat
    public float def = 5f; // Defence stat
    public float health = 10f; // Health stat
    public int maxMoves = 1; // The current number of moves left
    public int maxMaxMoves = 1; // The max amount of moves that can be made
    public bool unitIsSelectable = true; // Checks if the unit has moved yet
    public float travelDist = 4; // How far the unit can move

    public TurnController turnController;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (health <= 0) // If health drops below zero then destory
        {
            if (this.gameObject.tag == "PlayerUnit")
            {
                turnController.GetComponent<TurnController>().playerUnits -= 1;
            }
            if (this.gameObject.tag == "EnemyUnit")
            {
                turnController.GetComponent<TurnController>().enemyUnits -= 1;
            }
            Destroy(this.gameObject);
        }

        if (maxMoves <= 1) // If unit has passed its max moves then no longer selectable
        {
            unitIsSelectable = false;
        }
	}
}
