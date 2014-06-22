using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {


	// List<Vector3> viewpoints
	public Viewpoint viewpoint;
	//public enum Viewpoint{top,sideR,sideL,thirdPerson,firstPerson};

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

public enum Viewpoint{top,sideR,sideL,thirdPerson,firstPerson};
