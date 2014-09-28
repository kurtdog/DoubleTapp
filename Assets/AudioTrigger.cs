using UnityEngine;
using System.Collections;

public class AudioTrigger : MonoBehaviour
{

    public AudioClip audioClip;
    public GameObject ShooterShip; // the ship model
    public MusicManager musicManager;

    //public int triggerRange; // trigger the audio when the player gets within this range
    public bool audioTriggered = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(this.gameObject.transform.position, ShooterShip.transform.position) < this.audio.maxDistance && musicManager.fadeTrack == false)
        {
            audioTriggered = true;
            musicManager.FadeRemovableBackground(this.gameObject);
        }
        else // stop fading the background track once we've moved outside of the maxRange for this 3D sound.
        {
            if (musicManager.fadeTrack == true)
            {
                musicManager.fadeTrack = false;
                audioTriggered = false;
            }
        }
    }

}