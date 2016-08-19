using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileController : MonoBehaviour {

    public TurnController turnController; // It's LITERALLY in the name

    public Material notSelectable; // The material for when it's unselectable

    public Material selectable; // The material for when it's selectable

    public MeshRenderer meshRenderer; // The Tile mesh renderer

    public float thisUnitDistance; // The distance from unit to the tile

	// Use this for initialization
	void Start () {
        turnController = FindObjectOfType<TurnController>();
	}
	
	// Update is called once per frame
	void Update () {
        HighlightTile();

        thisUnitDistance = Vector3.Distance(turnController.GetComponent<TurnController>().selectedUnit.transform.position,
                                                                                        this.transform.position);
    }

    void HighlightTile() // Highlights tile based on the distance to the selected unit
    {
        if (turnController.GetComponent<TurnController>().unitSelected == true && thisUnitDistance <= turnController.GetComponent<TurnController>().selectedUnit.GetComponent<CapsuleUnit>().travelDist && turnController.GetComponent<TurnController>().selectedUnit.GetComponent<CapsuleUnit>().maxMoves >= 1)
        {
            meshRenderer.material = selectable;
        }
        else 
        {
            meshRenderer.material = notSelectable;
        }
    }
}
