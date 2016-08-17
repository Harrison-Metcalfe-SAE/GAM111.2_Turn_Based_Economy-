using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileController : MonoBehaviour {

    public TurnController turnController;

    public Material notSelectable;

    public Material selectable;

    public MeshRenderer meshRenderer;

    public float thisUnitDistance;

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

    void HighlightTile()
    {
        if (turnController.GetComponent<TurnController>().unitSelected == true && thisUnitDistance <= turnController.GetComponent<TurnController>().selectedUnit.GetComponent<CapsuleUnit>().travelDist)
        {
            meshRenderer.material = selectable;
        }
        else 
        {
            meshRenderer.material = notSelectable;
        }
    }
}
