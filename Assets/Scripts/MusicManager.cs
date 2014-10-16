using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour {

	public GameObject camera;
	public GameObject ShooterShip;
    public AudioSource[] audioTracks;
	public Color c1 = Color.yellow;
	public Color c2 = Color.red;
	public int	numLines;
	public float radius;
	public float lineWidth;
	public float waveformLineWidth;
    public float fadeTime;// the time it takes to fadeOut/In a track.
	private int audioSamples;
	float[] samples;
	int cameraOffset = 5;

	private int numColors;

	private int degPerLine;
	public int segPerLine;
	private float stepSize;
	private float x,y,z;

    public bool fadeTrack;
    private float fadeTimer;
    private float beatTimer;

    private AudioSource mainTrack;
    private AudioSource removeableTrack;

    private AudioSource extraTrack;
    private GameObject extraTrackTrigger;
	//private List<GameObject> colorLines;
	private List<GameObject> soundArcs;
	// Use this for initialization
	void Start () {
		//segPerLine = 200;
        audioTracks = GetComponents<AudioSource>();
        fadeTimer = fadeTime;
        beatTimer = 0;
        //mainTrack = audioTracks[0];
        //removeableTrack = audioTracks[1];
        /*
        for (int i = 0; i < audioTracks.Length; i++ )
        {
            audioTracks[i].Play();
        }
         * */

            soundArcs = new List<GameObject>();
		samples = new float[segPerLine]; //allocate space for audio samples
		CreateSoundArcs(numLines);

	}
	
	// Update is called once per frame
	void Update () {
		//DrawSoundArc(0,175);
		//DrawSoundArc(180,360);
        fadeTimer += Time.fixedDeltaTime;
        beatTimer += Time.fixedDeltaTime;
		DrawSoundArcs(0,360,numLines);

        if(beatTimer >= 8.33334f)
        {
            beatTimer = 0;
           // Debug.Log("Bumpity Bump Son");
        }

        if(fadeTrack)
        {
            float distFromObject = Vector3.Distance(this.transform.position, extraTrackTrigger.transform.position);
            float volumePercentage = Mathf.Min(1, (distFromObject - extraTrack.minDistance) / (extraTrack.maxDistance - extraTrack.minDistance));
            if (volumePercentage < 0)
            {
                volumePercentage = 0;
            }
            audioTracks[1].volume = volumePercentage;
            //Debug.Log("volume = " + volumePercentage);
        }
        /*
        if(fadeTimer < fadeTime)
        {
           
            float volumePercentage = 1 - (fadeTimer / fadeTime);
            audioTracks[1].volume = volumePercentage;
            Debug.Log("volume = " + volumePercentage);
        }
         * */
	}

	void CreateSoundArcs(int number)
	{
		//samples = new float[segPerLine]; //allocate space for audio samples
		//create x lines to be used as SoundArcs
		for(int i = 0; i < number; i++)
		{
			NewLine(c1,c2,soundArcs);
		}
		
		//Debug.Log("soundArcs: " + soundArcs.Count);
	}

    public void addAudioSource(AudioClip audioClip)
    {
        //new audiosource
    }

    public void FadeRemovableBackground(GameObject audioTrigger)
    {
        fadeTimer = 0;
        fadeTrack = true;
        extraTrackTrigger = audioTrigger;
        extraTrack = audioTrigger.GetComponent<AudioSource>();
    }

    public void StopFadingRemovableBackground()
    {
        fadeTrack = false;
    }

    public void SwitchRemovableBackground(AudioClip audioClip)
    {
        addAudioSource(audioClip);
        //start new music with volume 0
        //fade the new music in
        //fade the old music out
        //audioTracks[1]
    }

	public GameObject NewLine(Color color1, Color color2, List<GameObject> lineList)
	{
		GameObject go = new GameObject(); 
		lineList.Add(go);
		LineRenderer line = go.AddComponent<LineRenderer>();
		line = go.GetComponent<LineRenderer>();
		line.material = new Material(Shader.Find("Particles/Additive"));
		line.SetColors(color1, color2);
		line.SetWidth(lineWidth, lineWidth);
		line.SetVertexCount((int)segPerLine);
		
		return go;
	}

	void DrawSoundArcs(int startAngle, int endAngle, int numArcs)
	{
		int angle = Mathf.Abs(endAngle-startAngle);
		int step = angle/numArcs;
		int start = startAngle;
		int end = startAngle+step;
		for(int i = 0; i < numArcs; i++)
		{
			//GameObject newline = NewLine(c1,c2);
			//Debug.Log("drawing arc " + i +" from " + start + " to " + end);
			DrawSoundArc(start,end,soundArcs[i]);	
			start += step;
			end += step;
		}
		
	}
	
	void DrawSoundArc(float startAngle, float endAngle, GameObject soundArc)
	{
		float x = ShooterShip.transform.position.x;	
		float y = ShooterShip.transform.position.y; // 0
		float z = ShooterShip.transform.position.z; // -z
		
		
		float angle = startAngle;
		
		//radius = gameManagerScript.deathDistance;
		//GameObject newLine = NewLine(c1,c2);
		LineRenderer soundLine = soundArc.GetComponent<LineRenderer>();
		
		audioTracks[0].GetOutputData(samples, 0); // get samples from the audio

		float degPerLine = Mathf.Abs(endAngle - startAngle);
		stepSize = (float)degPerLine/segPerLine;

		//Debug.Log("stepSize = " + stepSize);
		//Debug.Log(" segPerLine = " + segPerLine);

		//Debug.Log("drawing arc from "+  startAngle + " to " + endAngle);
		for (int i = 0; i < segPerLine; i++)
		{
			float deg = Mathf.Deg2Rad * angle;
			float audioOffset = samples[i];
			float newRadius = (radius + audioOffset);
			x = ShooterShip.transform.position.x + Mathf.Sin (deg) * newRadius;
			y = ShooterShip.transform.position.y + Mathf.Cos (deg) * newRadius;
			Vector3 pos = new Vector3(x,y,z);
			//Debug.Log("pos: " + pos);            
			
			soundLine.SetPosition (i,pos);
			angle += stepSize;
			
		}
		
		
	}

}
