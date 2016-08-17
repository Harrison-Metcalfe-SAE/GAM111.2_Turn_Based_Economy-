using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CapsuleUnit : MonoBehaviour {

    public string playerClass = "Capsule";
    public float atk = 10f;
    public float def = 5f;
    public float health = 10f;
    public int maxMoves = 1;
    public int maxMaxMoves = 1;
    public bool unitIsSelectable = true;
    public float travelDist = 4;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }

        if (maxMoves <= 1)
        {
            unitIsSelectable = false;
        }
	}
}
