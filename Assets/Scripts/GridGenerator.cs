using UnityEngine;
using System.Collections;

public class GridGenerator : MonoBehaviour {

    public Vector2 gridSize;
    public GameObject tile;

	// Use this for initialization
	void Start ()
    {
        GenerateTiles();
	}
	
    void GenerateTiles()
    {
        Vector3 loc;

        for (int X = 0; X < gridSize.x; ++X)
        {
            for (int Z = 0; Z < gridSize.y; ++Z)
            {
                loc = new Vector3(this.transform.position.x + 0.5f + X, this.transform.position.y, this.transform.position.z + 0.5f + Z);
                Instantiate(tile, loc, tile.transform.rotation);
            }
        }
    }

}
