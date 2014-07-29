using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraScript : MonoBehaviour {

	public GameObject ShooterShip;
	public GameObject ViewPointNetwork;
	public GameObject currentView;
	public float adjustmentSpeed;
	public float movementSpeed;
	public float lerpSpeed;


	private GameObject sideViewL;
	private GameObject sideViewR;
	private GameObject topView;
	private GameObject thirdPersonView;
	public bool invertedX;
	public bool invertedY;
	//public bool transitioning = false;

	//public enum Viewpoint{top,sideR,sideL,thirdPerson,firstPerson};
	float radiusFromShooter;
	Vector3 offset;

	// Use this for initialization
	void Start () {
		InitializeViewpointObjects();
		radiusFromShooter = 1; //get a positionalOffset
		offset = ShooterShip.transform.position - thirdPersonView.transform.position;
		currentView = thirdPersonView;
		SwitchViewPoints(currentView); // switch to our selected viewpoint
	}
	
	// Update is called once per frame
	void Update () {

	
		FollowShip();//follow the player's position



		HandleInput();

	}
	//follow the player's position
	void FollowShip()
	{	
		if(currentView = thirdPersonView)
		{
			transform.position = ShooterShip.transform.position - (Camera.main.transform.rotation * offset);
		}
		else{
			transform.position = currentView.transform.position;
		}

	}


	//Slowly Lerp Back towards thirdPersonView
	void LerpRotation()
	{
		//---------------Lerp Position
		Vector3 xAxis = Camera.main.transform.right; //thirdPersonView
		Vector3 yAxis = Camera.main.transform.up;

		float xDistance = this.transform.position.x - ShooterShip.transform.localPosition.x;
		float yDistance = this.transform.position.y - ShooterShip.transform.localPosition.y;


		float threshold = 1;
		if(Mathf.Abs(xDistance) > threshold || Mathf.Abs(yDistance) > threshold)
		{
			//Camera.main.transform.RotateAround(ShooterShip.transform.position,yAxis,xDistance*lerpSpeed); // move the camera point
			//Camera.main.transform.RotateAround(ShooterShip.transform.position,xAxis,yDistance*lerpSpeed);
		}
		//-------------------Lerp Rotation
		//rotate the up vector to match the ShooterShip's
		int sign = 1;
		if(this.transform.up.x < 0)
		{
			sign = -1;
		}
		float angle = sign*Vector3.Angle(ShooterShip.transform.up,this.transform.up);

		Vector3.Lerp(this.transform.up,ShooterShip.transform.up,sign*lerpSpeed);
		Debug.Log("Distance: ( " + xDistance + " , " + yDistance + " )  Angle: " + angle);
	}

	//assign the viewpoints from the viewpointNetwork into our private viewpoint variables
	void InitializeViewpointObjects()
	{
		foreach(Transform child in ViewPointNetwork.transform)
		{
			ViewPointScript childViewpointScript = child.gameObject.GetComponent<ViewPointScript>();
			if( childViewpointScript != null)
			{
				if(childViewpointScript.viewPoint == Viewpoint.sideL)
				{
					sideViewL = child.gameObject;
				}
				if(childViewpointScript.viewPoint == Viewpoint.sideR)
				{
					sideViewR = child.gameObject;
				}
				if(childViewpointScript.viewPoint == Viewpoint.top)
				{
					topView = child.gameObject;
				}
				if(childViewpointScript.viewPoint == Viewpoint.thirdPerson)
				{
					thirdPersonView = child.gameObject;
				}
			}
		}

	}

	void HandleInput()
	{

		if(Input.GetButtonDown("Left"))
		{
			//viewpoint = Viewpoint.sideL;

			currentView = sideViewL;
			SwitchViewPoints(currentView);

		}
		if(Input.GetButtonDown("Top"))
		{
			//viewpoint = Viewpoint.top;
			currentView = topView;
			SwitchViewPoints(currentView);
		}
		if(Input.GetButtonDown("Right"))
		{
			//viewpoint = Viewpoint.sideR;
			currentView = sideViewR;
			SwitchViewPoints(currentView);
		}
		if(Input.GetButtonDown("3rdPerson"))
		{
			//viewpoint = Viewpoint.thirdPerson;
			currentView = thirdPersonView;
			SwitchViewPoints(currentView);
		}

		// move around the player in a unit sphere
		if(currentView == thirdPersonView)
		{
			//Camera.main.transform.LookAt(ShooterShip.transform.position);
			//currentView.transform.forward = (this.transform.position - ShooterShip.transform.position);

			float xJoystick;// = -Input.GetAxis("Horizontal2");
			float yJoystick;// = -Input.GetAxis("Vertical2");
			if(invertedX)
			{
				xJoystick = Input.GetAxis("Horizontal2");
			}
			else{
				xJoystick = -Input.GetAxis("Horizontal2");
			}
			if(invertedY)
			{
				yJoystick = Input.GetAxis("Vertical2");
			}
			else{
				yJoystick = -Input.GetAxis("Vertical2");
			}
			
			
			Vector3 xAxis = Camera.main.transform.right;
			Vector3 yAxis = Camera.main.transform.up;

			if(Mathf.Abs(xJoystick) > .8f || Mathf.Abs(yJoystick) > .8f)
			{
				//Debug.Log("rotating x by : " + xJoystick*movementSpeed);
				//Debug.Log("rotating y by : " + yJoystick*movementSpeed);
				Camera.main.transform.RotateAround(ShooterShip.transform.position,yAxis,xJoystick*movementSpeed); // move the camera point
				Camera.main.transform.RotateAround(ShooterShip.transform.position,xAxis,yJoystick*movementSpeed);
				//currentView.transform.LookAt(ShooterShip.transform.position);//
			
			}
			else{
				LerpRotation();
			}
			//Camera.main.transform.LookAt(ShooterShip.transform,this.transform.up);
		}

	}

	void SwitchViewPoints(GameObject vp)
	{
		//Debug.Log("Switchin vp to: " + vp.name);
		this.transform.rotation = currentView.transform.rotation;
		this.transform.position = currentView.transform.position;

		/*

		*/

	}

	/*
	void MoveCamera(GameObject go)
	{
		//Debug.Log("setting position = " + go.position);
		//adjust rotation and position
		this.transform.rotation = go.transform.rotation;
		this.transform.position = go.transform.position;
		transitioning = false;
		
	}*/
	
}

public enum Viewpoint{top,sideR,sideL,thirdPerson};
