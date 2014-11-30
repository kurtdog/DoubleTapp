using UnityEngine;
using System.Collections;

public class AudioScript : MonoBehaviour {

    public GameObject Ship;
    public AudioClip audioClip;

    private AudioSource audioSource;
	// Use this for initialization
	void Start () {
        audioSource = Ship.GetComponent<AudioSource>(); //get the audio source attached to the ship
        audioSource.clip = audioClip; //set the audioClip on the ship, = to the audioclip attached to this script
	}
	

    public void PlayAudio()
    {
        audioSource.clip = audioClip; //set the audioClip on the ship, = to the audioclip attached to this script. we do this again in case something else changed it.
        Ship.GetComponent<AudioSource>().Play();
    }

    //this.GetComponent<AudioScript>().PlayAudio(); // put this line wherever you want to play the sound
}
