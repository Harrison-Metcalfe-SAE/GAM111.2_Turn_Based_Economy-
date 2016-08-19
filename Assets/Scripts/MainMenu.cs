using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Button onePlayer; // 1 player
    public Button twoPlayer; // 2 player
    public Button credits; // Credits
    public Button Quit; // Quit
    public Text dlc; // Coming soon
    public AudioManager audioManager; // Audio Manager
    public AudioClip mainTheme; // Main menu music

	// Use this for initialization
	void Start () {
        audioManager.playAudio(mainTheme);
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void OnePlayerStart()
    {
        /*
         * In all seriousness, I failed to implement a single player mode and AI due to time restraints and poor planning
         */
        dlc.text = "Single Player DLC out February 31st";
    }

    public void TwoPlayerStart()
    {
        Application.LoadLevel("Game");
    }

    public void Credits()
    {
        Application.LoadLevel("Credits");
    }
}
