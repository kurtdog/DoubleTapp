using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraScript : MonoBehaviour {

	public GameObject Ship;
	public GameObject ShooterShip;
	public GameObject ViewPointNetwork;
	public GameObject currentView;
	public float adjustmentSpeed;
	public float movementSpeed;
	public float lerpSpeed;
	public float lerpTreshold;

	private GameObject sideViewL;
	private GameObject sideViewR;
	private GameObject topView;
	private GameObject thirdPersonView;
	private ShipController shipController;
	//in degrees
	private float roll;
	private float pitch;
	private float yaw;
	private float lastYaw;

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
		shipController = Ship.GetComponent<ShipController>();
	}
	
	// Update is called once per frame
	void Update () {

	
		FollowShip();//follow the player's position



		HandleInput();

	}
	//follow the player's xyz position
	void FollowShip()
	{	
		if(currentView == thirdPersonView)
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
		int pitchSign = 1;
		if(this.transform.up.x < 0)
		{
			pitchSign = -1;
		}
		int yawSign = 1;
		if(this.transform.forward.x < 0)
		{
			yawSign = -1;
		}
		pitch = pitchSign*Vector3.Angle(ShooterShip.transform.up,this.transform.up);
		yaw = yawSign*Vector3.Angle(ShooterShip.transform.forward,this.transform.forward);

		//Vector3.Lerp(this.transform.up,ShooterShip.transform.up,sign*lerpSpeed);
		//Debug.Log("Distance: ( " + xDistance + " , " + yDistance + " )  upAngle: " + upAngle +" forwardAngle: " + forwardAngle);
		//Debug.Log("pitch: " + pitch +" yaw: " + yaw + " DeltaYaw " + Mathf.Abs(yaw-lastYaw));



		//stepSize
		float step = lerpSpeed*Time.deltaTime;

		if(Mathf.Abs(yaw-lastYaw) > lerpTreshold  ) //|| Mathf.Abs(yaw) > 5f
		{
			Vector3 newForward = Vector3.RotateTowards(this.transform.forward,ShooterShip.transform.forward,step,0.0f);
			transform.rotation = Quaternion.LookRotation(newForward);
			
		}
		if(Mathf.Abs(pitch) > lerpTreshold  )
		{

		}
		lastYaw = yaw;
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
		//Controls that can be done whenever------------------------------
		if(Input.GetButtonDown("3rdPerson")) // switch to third person
		{
			SwitchViewPoints(thirdPersonView);
		}
		//Controls that can be done when locked on------------------------------
		if(shipController.lockedOn) 
		{

			//be able to switch viewpoints
			if(Input.GetButtonDown("Left"))
			{
				SwitchViewPoints(sideViewL);
			}
			if(Input.GetButtonDown("Top"))
			{
				SwitchViewPoints(topView);
			}
			if(Input.GetButtonDown("Right"))
			{
				SwitchViewPoints(sideViewR);
			}


			lockedOnCamera();
		}
		//Controls that can be done in thirdperson mode------------------------------
		else{ // if not locked on, move to thirdpersonview
			if(currentView != thirdPersonView)
			{
				currentView = thirdPersonView;
				SwitchViewPoints(currentView);
			}

			thirdPersonCamera();
		
		}


	}

	void lockedOnCamera()
	{
		//Debug.Log("LockedOn Camera");
		//adjust rotation and position
		this.transform.rotation = currentView.transform.rotation;
		this.transform.position = currentView.transform.position;
		//transitioning = false;
	}

	void thirdPersonCamera()
	{
		//Debug.Log("ThirdPerson Camera");
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

	void SwitchViewPoints(GameObject vp)
	{
		//Debug.Log("Switchin vp to: " + vp.name);
		currentView = vp;
		this.transform.rotation = currentView.transform.rotation;
		this.transform.position = currentView.transform.position;

	}

	/*
	void MoveCamera(GameObject go)
	{

		
	}*/
	
}

public enum Viewpoint{top,sideR,sideL,thirdPerson};
