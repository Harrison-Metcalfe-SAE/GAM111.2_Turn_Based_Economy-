using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

    public TurnController UI;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    if(UI.enemyTurn == true)
        {
            Debug.Log("Enemy Phase");
        }
	}
}
