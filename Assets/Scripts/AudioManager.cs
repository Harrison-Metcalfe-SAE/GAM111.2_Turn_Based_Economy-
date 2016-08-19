using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    public static AudioManager audioManager; // LITERALLY in the name
    public AudioSource audioSource; 

    // Use this for initialization
    void Awake () {
        audioManager = this;

	}
	
	// Update is called once per frame
	void Update () {

	}

    public void playAudio(AudioClip soundEffect) // Plays the music
    {
        audioSource.clip = soundEffect;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void stopAudio() // Stops the music
    {
        audioSource.Stop();
    }
}
