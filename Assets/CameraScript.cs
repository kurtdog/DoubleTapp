using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraScript : MonoBehaviour {

	public GameObject ShooterShip;
	public GameObject ViewPointNetwork;
	public List<GameObject> viewpointObjects;
	public Viewpoint viewpoint;
	private Viewpoint lastViewpoint;
	public float adjustmentSpeed;
	public bool transitioning = false;

	//public enum Viewpoint{top,sideR,sideL,thirdPerson,firstPerson};

	// Use this for initialization
	void Start () {
		foreach(Transform child in ViewPointNetwork.transform)
		{
			viewpointObjects.Add(child.gameObject);
		}
		lastViewpoint = viewpoint;
		SwitchViewPoints(viewpoint); // switch to our selected viewpoint
	}
	
	// Update is called once per frame
	void Update () {


		if(lastViewpoint != viewpoint)
		{	
			lastViewpoint = viewpoint;
			SwitchViewPoints(viewpoint);

		}

		HandleInput();

	}

	void HandleInput()
	{
		if(Input.GetButtonDown("Left"))
		{
			viewpoint = Viewpoint.sideL;
		}
		if(Input.GetButtonDown("Top"))
		{
			viewpoint = Viewpoint.top;
		}
		if(Input.GetButtonDown("Right"))
		{
			viewpoint = Viewpoint.sideR;
		}
		if(Input.GetButtonDown("3rdPerson"))
		{
			viewpoint = Viewpoint.thirdPerson;
		}



	}

	void SwitchViewPoints(Viewpoint vp)
	{
		Debug.Log("Switchin to vp: " + vp);
		foreach(Transform child in ViewPointNetwork.transform)
		{
			ViewPointScript childViewpointScript = child.gameObject.GetComponent<ViewPointScript>();
			if( childViewpointScript != null)
			{
				if(childViewpointScript.viewPoint == vp)
				{
					Debug.Log("Moving camera to " + child.gameObject.name);
					MoveCamera(child.gameObject);
					transitioning = true;

				}
			}
		}

	}

	//move the camera to align with the go's position and rotation
	void MoveCamera(GameObject go)
	{
		//Debug.Log("setting position = " + go.position);
		//adjust rotation
		this.transform.rotation = go.transform.rotation;
		//this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles,go.eulerAngles,Time.fixedDeltaTime*adjustmentSpeed);
		if(viewpoint == Viewpoint.thirdPerson)
		{
			//Camera.main.transform.LookAt(ShooterShip.transform,this.transform.up);
		}
	
			
	


		//adjust position
		this.transform.position = go.transform.position;
		//this.transform.position = Vector3.Lerp(this.transform.position,go.transform.position,Time.fixedDeltaTime*adjustmentSpeed);

		transitioning = false;
		
	}
	
}

public enum Viewpoint{top,sideR,sideL,thirdPerson};
