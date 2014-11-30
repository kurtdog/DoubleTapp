using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ThirdPersonMovement
{
    public float xLerpSpeed;
    public float yLerpSpeed;
    public float zLerpSpeed;
    public float hyperLerpSpeed;

    public float xAxisLerpSpeed;
    public float yAxisLerpSpeed;
    public float zAxisLerpSpeed;
    //used to keep angles from being 0, it causes errors with the lerp system.
    public float angleThreshold;
}


public class CameraScript : MonoBehaviour {

	public GameObject Ship;
	private GameObject ShooterTarget;
	public GameObject ViewPointNetwork;
	public GameObject currentView;
    public ThirdPersonMovement thirdPersonMovement;
	public float adjustmentSpeed;
	public float movementSpeed;
	public float lerpSpeed;
	public float lerpStartThreshold;
    public float lerpEndThreshold;
    private bool lerp = false;

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
    private float lastRoll;

    //public int startLerpThreshold; // distance at which we follow the ship directly;


    float lastVelocity;
    Vector3 lastOffset;
    float velocityX;
    float velocityY;
    float velocityZ;

    float xOffset;
    float yOffset;
    float zOffset;
    float initialAngleX;
    //private float lastPitch;

	public bool invertedX;
	public bool invertedY;

	public float zoom; //0-1 indicating how much of the screen the gameObject and it's target take up.
	public float maxDistance;
	//public bool transitioning = false;

	//public enum Viewpoint{top,sideR,sideL,thirdPerson,firstPerson};
	public float distanceFromShooter;
    
	Vector3 offset;

	// Use this for initialization
	void Start () {
		InitializeViewpointObjects();
		distanceFromShooter = Vector3.Distance(this.transform.position,currentView.transform.position);
		offset = Ship.transform.position - thirdPersonView.transform.position;
		currentView = thirdPersonView;
		SwitchViewPoints(currentView); // switch to our selected viewpoint
		shipController = Ship.GetComponent<ShipController>();

        offset = this.transform.position - Ship.transform.position;
        Vector3 desiredPosition = Ship.transform.position + Ship.transform.right * offset.x + Ship.transform.up * offset.y + Ship.transform.forward * offset.z;
        xOffset = desiredPosition.x;
        yOffset = desiredPosition.y;
        zOffset = desiredPosition.z;

        initialAngleX = this.transform.eulerAngles.x;
	}

    // Update is called once per frame
    void Update()
    {

        //TODO: turn this off for the menu screen
        FollowShip();//follow the player's position

        HandleInput();

    }

    void LateUpdate()
    {
        if (currentView == thirdPersonView)
        {
            FollowPlayer();
        }

    }

    void FollowPlayer()
    {
        Vector3 desiredPosition = Ship.transform.position + Ship.transform.right * offset.x + Ship.transform.up * offset.y + Ship.transform.forward * offset.z;
        //this.transform.position = target.transform.position + offset;

        if (Input.GetButton("HyperThrust"))
        {
            xOffset = Mathf.Lerp(xOffset, desiredPosition.x, thirdPersonMovement.hyperLerpSpeed * Time.fixedDeltaTime);
            yOffset = Mathf.Lerp(yOffset, desiredPosition.y, thirdPersonMovement.hyperLerpSpeed * Time.fixedDeltaTime);
            zOffset = Mathf.Lerp(zOffset, desiredPosition.z, thirdPersonMovement.hyperLerpSpeed * Time.fixedDeltaTime);
        }
        else
        {
            xOffset = Mathf.Lerp(xOffset, desiredPosition.x, thirdPersonMovement.xLerpSpeed * Time.fixedDeltaTime);
            yOffset = Mathf.Lerp(yOffset, desiredPosition.y, thirdPersonMovement.yLerpSpeed * Time.fixedDeltaTime);
            zOffset = Mathf.Lerp(zOffset, desiredPosition.z, thirdPersonMovement.zLerpSpeed * Time.fixedDeltaTime);
        }

        this.transform.position = new Vector3(xOffset, yOffset, zOffset);//


        LerpAngle();
    }

    void LerpAngle()
    {
        // get the lookatVector
        Vector3 lookAt = (Ship.transform.position - this.transform.position).normalized;
        //lerp the x,y, and z components of this.transform.forward to match the lookAt x, y, and z
        float xAngle = Mathf.SmoothDampAngle(this.transform.forward.x, lookAt.x, ref velocityX, thirdPersonMovement.xAxisLerpSpeed * Time.fixedDeltaTime);
        float yAngle = Mathf.SmoothDampAngle(this.transform.forward.y, lookAt.y, ref velocityY, thirdPersonMovement.yAxisLerpSpeed * Time.fixedDeltaTime);
        float zAngle = Mathf.SmoothDampAngle(this.transform.forward.z, lookAt.z, ref velocityZ, thirdPersonMovement.zAxisLerpSpeed * Time.fixedDeltaTime);
        this.transform.forward = new Vector3(xAngle, yAngle, zAngle);


        //Rotate around the z axis to keep the camera.up close to the target.up
        float deltaZ = (Ship.transform.eulerAngles.z - this.transform.eulerAngles.z);
        float desiredAngleZ = Ship.transform.eulerAngles.z;
        float currentZ = Mathf.Lerp(this.transform.eulerAngles.z, desiredAngleZ, thirdPersonMovement.zAxisLerpSpeed);
        //Debug.Log ("desiredZ: "  + desiredAngleZ + " currentZ: " + currentZ  + " deltaZ: " + deltaZ);
        this.transform.rotation = Quaternion.Euler(this.transform.eulerAngles.x, this.transform.eulerAngles.y, currentZ);//Quaternion.Euler(currentX, currentY, currentZ);


    }


    //follow the player's xyz position
    void FollowShip()
    {
        if (!shipController.lockedOn)
        {
            currentView = thirdPersonView;
        }


        if (currentView != thirdPersonView)
        {
            transform.position = currentView.transform.position;
            AdjustDistance(); // adjust out distance so that both items
        }

    }
	void AdjustDistance()
	{
		if(shipController.lockedOn)
		{
			ShooterTarget = Ship.GetComponent<Shooter>().target.gameObject;
			//Debug.Log("Items On Screen: " + ItemsOnScreen());
			/*
			if(!ItemsOnScreen()) // if the shooter ship and it's target are both on screen
			{
				LerpDistance(); // lerpdistance will check the distance from teh player to the object in worldspace. And lerp to make it a given distance in screen space
			}*/

			LerpDistance();
		}



	}

	// return true if the shootership and it's target are on screen, false otherwise
	bool ItemsOnScreen()
	{
		if(Ship.renderer.isVisible && Ship.GetComponent<Shooter>().target.renderer.isVisible)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	// lerpdistance will check the distance from teh player to the object in worldspace. And lerp to make it a given distance in screen space
	void LerpDistance()
	{
		//get the distance of the two objects
		float shipToTarget = Vector3.Distance(Ship.transform.position,Ship.GetComponent<Shooter>().target.transform.position);
		// we want worldDistance/cameraDistance = zoom. therefor cameraDistance = worldDist/zoom
		float cameraDistance;
		if(zoom != 0)
		{
			cameraDistance = shipToTarget/zoom; // get the distance we need to put our camera at
		}
		else{

			cameraDistance = shipToTarget/.5f;

		}

		//Debug.Log("cameraDist: " + cameraDistance);
		//hardsetCamera
		Vector3 midpoint =Ship.transform.position + Ship.transform.forward*shipToTarget/2; // halfway between the ship and it's target, 
		currentView.transform.position = midpoint - this.transform.forward*(cameraDistance); // + z distance


	}




	//Slowly Lerp Back towards thirdPersonView
	void LerpRotation()
	{
        float xJoystick = Input.GetAxis("Horizontal");
        float yJoystick = Input.GetAxis("Vertical");


		int yawSign = 1;
		if(this.transform.forward.x < 0)
		{
			yawSign = -1;
		}
		yaw = yawSign*Vector3.Angle(Ship.transform.forward,this.transform.forward);
		//Vector3.Lerp(this.transform.up,ShooterShip.transform.up,sign*lerpSpeed);
		//Debug.Log("Distance: ( " + xDistance + " , " + yDistance + " )  upAngle: " + upAngle +" forwardAngle: " + forwardAngle);
		//Debug.Log("pitch: " + pitch +" yaw: " + yaw + " roll: " + roll);// + " DeltaYaw " + Mathf.Abs(yaw-lastYaw));



		//stepSize
		float step = lerpSpeed*Time.deltaTime;

		if(Mathf.Abs(yaw) > lerpStartThreshold) //lerp when the yaw is > than our start threshold. //Mathf.Abs(yaw-lastYaw) > lerpTreshold 
		{
            Debug.Log("start");
            lerp = true;

		}
        if (Mathf.Abs(yaw) < lerpEndThreshold)//stop lerp when the yaw is < than our end threshold. 
        {
            Debug.Log("end");
            lerp = false;
        }

        if(lerp)
        {
            //TODO: I think the lerping problems are stemming form this rotateTowards part.
            //Basically doing a 'lookAt' // this is old shitty code, but it works lol
            Vector3 newForward = Vector3.RotateTowards(this.transform.forward, Ship.transform.forward, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newForward);
        }
		else if(Mathf.Abs(xJoystick) < .6 && Mathf.Abs(yJoystick) < .6)// only fix roll whne not fixing yaw
		{

            //SPIN THE CAMERA TO line up with the plaer
            int rollSign = 1;
            Vector3 rollCheck = Vector3.Cross(this.transform.up, Ship.transform.up);
            float vectorCompare = (rollCheck.normalized - Ship.transform.forward.normalized).magnitude;
            if (vectorCompare > 1)
            {
                rollSign = -1;
            }
            // Debug.Log("RotateClockwise");
            roll = rollSign * Vector3.Angle(Ship.transform.up, this.transform.up);
            pitch = Vector3.Angle(this.transform.forward, Ship.transform.forward);
            Debug.Log("Roll:" + roll + " Pitch:" + pitch);
            if (Mathf.Abs(pitch) < 5 && Mathf.Abs(roll) > 2) //&& Mathf.Abs(xJoystick2) < .5// if the camera is ligned up AND we need to roll to adjust our up direction..
            {
                //Rotate the ship to face up.
                Debug.Log("Rotating");
                this.transform.RotateAround(this.transform.position, this.transform.forward, 2 * rollSign);
            }
		}
		lastYaw = yaw;
        lastRoll = roll;
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
       
       // float xJoystick = Input.GetAxis("Horizontal");
        //float yJoystick = Input.GetAxis("Vertical");
		float xJoystick2 = Input.GetAxis("Horizontal2");
		float yJoystick2 = Input.GetAxis("Vertical2");

		
		
		Vector3 xAxis = Camera.main.transform.right;
		Vector3 yAxis = Camera.main.transform.up;

		
		if(Mathf.Abs(xJoystick2) > .8f || Mathf.Abs(yJoystick2) > .8f)
		{
			//Debug.Log("rotating x by : " + xJoystick*movementSpeed);
			//Debug.Log("rotating y by : " + yJoystick*movementSpeed);
            //COMMENTED OUT FOR SHIPCONTROLLER - SPIN MOVE
            /*
			Camera.main.transform.RotateAround(ShooterShip.transform.position,yAxis,xJoystick2*movementSpeed); // move the camera point
			Camera.main.transform.RotateAround(ShooterShip.transform.position,xAxis,yJoystick2*movementSpeed);
			*/
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
