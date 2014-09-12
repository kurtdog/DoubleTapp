using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LockWheelSc : MonoBehaviour {

	public GameObject camera;
	public GameObject ShooterShip;
	public Color c1 = Color.yellow;
	public Color c2 = Color.red;
	public int	numLines;
	public float radius;
	public float lineWidth;
	public float waveformLineWidth;
	private int audioSamples;
	float[] samples;
	int cameraOffset = 5;

	private int numColors;

	private int degPerLine;
	public int segPerLine;
	private float stepSize;
	private float x,y,z;


	//private List<GameObject> colorLines;
	private List<GameObject> soundArcs;
	// Use this for initialization
	void Start () {
		//segPerLine = 200;
		soundArcs = new List<GameObject>();
		samples = new float[segPerLine]; //allocate space for audio samples
		CreateSoundArcs(numLines);

	}
	
	// Update is called once per frame
	void Update () {
		//DrawSoundArc(0,175);
		//DrawSoundArc(180,360);

		DrawSoundArcs(0,360,numLines);
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
		
		audio.GetOutputData(samples, 0); // get samples from the audio

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
