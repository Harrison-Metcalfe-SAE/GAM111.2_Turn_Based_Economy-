using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    public static AudioManager audioManager;
    public AudioSource audioSource;

    public TurnController turnController;

    // Use this for initialization
    void Awake () {
        audioManager = this;

	}
	
	// Update is called once per frame
	void Update () {

	}

    public void playAudio(AudioClip soundEffect)
    {
        audioSource.clip = soundEffect;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void stopAudio()
    {
        audioSource.Stop();
    }
}
