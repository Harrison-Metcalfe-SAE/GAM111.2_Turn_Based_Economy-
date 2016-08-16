using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CapsuleUnit : MonoBehaviour {

    public static string playerClass = "Capsule";
    public static float atk = 10f;
    public static float def = 5f;
    public static float health = 10f;
    public static float maxMoves = 1f;
    public static bool unitIsSelectable = true;

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
