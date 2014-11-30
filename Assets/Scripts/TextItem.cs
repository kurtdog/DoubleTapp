using UnityEngine;
using System.Collections;

public class TextItem : MonoBehaviour {

	public string text;
	public Display displayType;
	public GameObject Target;
	public GameObject Target2;
	public enum Display{DISTANCE,TARGET2NAME, CUSTOM};

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		UpdateText ();

		//face the target
		Vector3 targetToMe = this.transform.position - Target.transform.position;
		this.transform.forward = targetToMe;


	}

	void UpdateText()
	{
		if(displayType == Display.DISTANCE)
		{
			float distance = Vector3.Distance (Target.transform.position, Target2.transform.position);
			Debug.Log("Distance:" + distance);
			text = "distance " + distance.ToString(); 
		}

		//overrite what's curren't in the textMesh if we need to
		if(this.GetComponent<TextMesh>().text != text)
		{
			this.GetComponent<TextMesh>().text = text;
		}
	}
}
