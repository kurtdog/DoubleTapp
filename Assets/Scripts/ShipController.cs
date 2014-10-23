using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ShipController : MonoBehaviour {

	public Camera camera;
	//public GameObject Projectile;
	//public GameObject ShotPosition;
	public GameObject ShooterShip;
    public GameObject thrustEffectGroup;
	//public Transform shooterScript.target;
	public float speed;
    public float hyperSpeed;
	//public float maxSpeed;
	public float currentSpeed;
	public float fireRate;
	public float turnSpeed;

	public float twoDspeed;
	public float rotationSlow;
	public float rotationSpeed3D;
    public float rollSpeed;
	public float aimRotationSpeed;
	public float rotationSpeed2D;
	private float currentRotationSpeed;
	public bool invertX;
	public bool invertY;
	public bool lockedOn;
	public float joystickThreshold;

	//public ShipType shipType;
	public enum ShipType{Shooter,Shield};
	
	private float xJoystick;
	private float yJoystick;
	private float xJoystick2;
	private float yJoystick2;



	private float shotTimer;
	int invX;
	int invY;
	CameraScript cameraScript;
	Shooter shooterScript;
	// Use this for initialization

//	public GameItem gameItem;
	void Start () {
		//gameItem = (GameItem)GetComponent(typeof(GameItem));

		lockedOn = false;
		cameraScript = camera.GetComponent<CameraScript>();
		shooterScript = this.GetComponent<Shooter>();
		currentRotationSpeed = rotationSpeed3D;

		invX = 1;
		invY = 1;
		if(invertX)
		{
			invX = -1;
		}
		if(invertY)
		{
			invY = -1;
		}

	}
	
	// Update is called once per frame
	public void Update () {


		//LockRotation();
		GetKeyInput(); // take action on button press events
		HandleControls();




		if(lockedOn)
		{
			this.transform.LookAt(shooterScript.target.transform.position); // lookAtit
		}

	}

	//TODO: get this working
	//not sure if I want to implement this. Maybe have it toggle-able
	void FollowTarget() // if we're locked on, we want to follow our shooterScript.target, i.e. stay within a certain distance of it. When the shooterScript.target moves, we do too
	{	
		Vector3 offset = shooterScript.target.position - cameraScript.currentView.transform.position;
		if(cameraScript.currentView.GetComponent<ViewPointScript>().viewPoint == Viewpoint.thirdPerson)
		{
			transform.position = shooterScript.target.position - (this.transform.rotation * offset);
		}
		else{
			transform.position = cameraScript.currentView.transform.position;
		}
		
	}

	//Button Press Events
	void GetKeyInput()
	{
		

		
		//Input.GetAxis("Thrust");
		// < 0 is Left Trigger
		// > 0 is Right Trigger

		if(Input.GetButtonDown("SlowAim"))
		{
			currentRotationSpeed = aimRotationSpeed;
		}
		if(Input.GetButtonUp("SlowAim"))
		{
			currentRotationSpeed = rotationSpeed3D;
		}

		if(Input.GetButtonDown("LockOn"))
		{

			lockedOn = !lockedOn;
		}
		if(shooterScript.target == null) // if the shooterScript.target exists
		{
			lockedOn = false;
		}


		if(Input.GetAxis("Thrust") > .5f)//rightTrigger
		{
			this.rigidbody.AddForce(this.transform.forward*speed*Input.GetAxis("Thrust"));

		}

        if (Input.GetButton("HyperThrust"))//rightTrigger
        {
            this.rigidbody.AddForce(this.transform.forward * hyperSpeed);
            Debug.Log("Pressing");

        }


		//Debug.Log("fire: " + Input.GetAxis("Fire1"));
		if(Input.GetAxis("Fire1") > .5f )//leftTrigger
		{
			//Shoot();
			//Debug.Log("Trying to shoot");
			this.GetComponent<Shooter>().Shoot();
		}
		//iff pressing back, and thrust, move backwards
		if(Input.GetButton("Thrust") && xJoystick < 0)
		{
			this.rigidbody.AddForce(-this.transform.forward*speed);
		}



        if (rigidbody.angularVelocity.magnitude > .1f)
        {
            rigidbody.AddTorque(-rigidbody.angularVelocity * rotationSlow * Time.fixedDeltaTime);
            //Debug.Log("adding torque:");
        }
	}

	/*
	 * TODO: A shoot that works with a ShotPointNetwork
	 * for each shotPosition in shotPOintNetwork.children
	 * {
	 * 	Shoot(shotPosition)
	 * }
	 * 
	 * */
	/*
	void Shoot()
	{
		//Debug.Log("shooting");
		shotTimer = 0;
		
		GameObject bullet = Instantiate(Projectile, ShotPosition.transform.position,this.transform.rotation) as GameObject;
		bullet.GetComponent<Projectile>().setParentGameObject(this.gameObject);
		
		//Debug.Log("speed: " + bullet.GetComponent<Projectile>().speed);
		//Debug.Log("forward: " + transform.forward);
		
		Vector3 f = ShotPosition.transform.forward*(bullet.GetComponent<Projectile>().speed +Mathf.Abs(this.rigidbody.velocity.magnitude));
		//bullet.GetComponent<Projectile>().force = f;
		//Debug.Log("adding force f: " + f);
		bullet.rigidbody.AddForce(f);
		//bullet.rigidbody.velocity = f;
		projectiles.Add(bullet);
	}
	*/
	//restrict movement in the Z axis
	//controls act as an arcade style 2D game in this mode
	void HandleControls()
	{
		//if(cameraScript.currentView.GetComponent<ViewPointScript>().viewPoint == Viewpoint.thirdPerson) // if in thirdPersonMode
		//{
		if(!lockedOn)
		{
			threeDMovement();

		}
		//otherwise we want 2D movement
		else{

			twoDMovement();
		}

	}

	void threeDMovement()
	{
		//Debug.Log("3D Movement");
		xJoystick = Input.GetAxis("Horizontal");
		yJoystick = Input.GetAxis("Vertical");
		xJoystick2 = Input.GetAxis("Horizontal2");
		yJoystick2 = Input.GetAxis("Vertical2");
		
		
		currentSpeed = rigidbody.velocity.magnitude; // view the velocity in the inspector
		Vector3 force = new Vector3(0,0,0);
		// if we're giving input

		//rotate ship
		//in third person View, fly around, rotate the ship, move freely in 3D space
		//flip
		if(Mathf.Abs(xJoystick) > joystickThreshold) 
		{
			//rigidbody.AddForce(force);
			
			Vector3 torqueVector = Camera.main.transform.up*invX*xJoystick*currentRotationSpeed;
			rigidbody.AddTorque(torqueVector);

			//torqueVector = this.transform.forward*xJoystick*rotationSpeed3D; // barrel roll
			//rigidbody.AddTorque(torqueVector);
			
		}
		// look left-right
		if(Mathf.Abs(yJoystick) > joystickThreshold)// 
		{
			Vector3 torqueVector = Camera.main.transform.right*yJoystick*invY*currentRotationSpeed; 
			rigidbody.AddTorque(torqueVector);
		}


        //SPIN MOVE USING JOYSTICK
        if (Mathf.Abs(xJoystick2) > .8f)
        {
            this.transform.RotateAround(this.transform.position, this.transform.forward, rollSpeed * -xJoystick2);
        }

	}

	void twoDMovement()
	{
		//Debug.Log("2D Movement");
		xJoystick = Input.GetAxis("Horizontal");
		yJoystick = Input.GetAxis("Vertical");
		//xJoystick2 = Input.GetAxis("Horizontal2");
		yJoystick2 = Input.GetAxis("Vertical2");
		//Debug.Log("yjoystick2 " + yJoystick2);
		//moving = false;

		Viewpoint currentViewpoint = cameraScript.currentView.GetComponent<ViewPointScript>().viewPoint;
		//third person controls
		if( currentViewpoint == Viewpoint.thirdPerson)
		{
			if(Mathf.Abs(xJoystick) > joystickThreshold || Mathf.Abs(yJoystick) > joystickThreshold ) 
			{
				//float distance = Vector3.Distance(shooterScript.target.transform.position,ShooterShip.transform.position);
				//Debug.Log("distance: " + distance);
				//use L-Joystick to move in a circle around your shooterScript.target
				float xRotation = xJoystick*rotationSpeed2D;//*(100.0f/distance); // rotate slower as you move away, and faster as you move close
				float yRotation = yJoystick*rotationSpeed2D;//*(100.0f/distance);
				transform.RotateAround(shooterScript.target.transform.position,Camera.main.transform.up,-xRotation);
				transform.RotateAround(shooterScript.target.transform.position,Camera.main.transform.right,-yRotation);
			}
			if(Mathf.Abs(yJoystick2) > joystickThreshold)
			{
				//use R-Joystick to move in and out
				float force = speed*-yJoystick2;
				Debug.Log("force: " + force);
				this.rigidbody.AddForce(this.transform.forward*force);
			}

	
		}
		// side and top controls
		else if(currentViewpoint == Viewpoint.sideR){
			if(Mathf.Abs(xJoystick) > joystickThreshold) 
			{
				//instead of case based movement we want to always move the ship based on the camera, right, left, up down, it's always from the camera's perspective
				//add force for xJoystick
				Vector3 MovementVector = camera.transform.right*xJoystick;// + camera.transform.up*-yJoystick;
				Vector3 force = MovementVector*twoDspeed;//*Time.fixedDeltaTime;
				rigidbody.AddForce(force);

			}
			if(Mathf.Abs(yJoystick) > joystickThreshold ) 
			{
				//rotate for yJoystick
				float yRotation = yJoystick*rotationSpeed2D;
				//Debug.Log("yRotation: " + yRotation);
				transform.RotateAround(shooterScript.target.transform.position,Camera.main.transform.forward,yRotation);

			}
			
		}
		else if(currentViewpoint == Viewpoint.sideL){
			if(Mathf.Abs(xJoystick) > joystickThreshold) 
			{
				//instead of case based movement we want to always move the ship based on the camera, right, left, up down, it's always from the camera's perspective
				//add force for xJoystick
				Vector3 MovementVector = camera.transform.right*xJoystick;// + camera.transform.up*-yJoystick;
				Vector3 force = MovementVector*twoDspeed;//*Time.fixedDeltaTime;
				rigidbody.AddForce(force);
				
			}
			if(Mathf.Abs(yJoystick) > joystickThreshold ) 
			{
				//rotate for yJoystick
				float yRotation = yJoystick*rotationSpeed2D;
				//Debug.Log("yRotation: " + yRotation);
				transform.RotateAround(shooterScript.target.transform.position,Camera.main.transform.forward,-yRotation);
			}
			
		}
		else if(currentViewpoint == Viewpoint.top){
			if(Mathf.Abs(xJoystick) > joystickThreshold) 
			{
				//instead of case based movement we want to always move the ship based on the camera, right, left, up down, it's always from the camera's perspective
				//rotate for xJoystick
				float xRotation = xJoystick*rotationSpeed2D;
				//Debug.Log("xRotation: " + xRotation);
				transform.RotateAround(shooterScript.target.transform.position,Camera.main.transform.forward,xRotation);
			}
			if(Mathf.Abs(yJoystick) > joystickThreshold ) 
			{
				//add force for yJoystick
				Vector3 MovementVector = camera.transform.up*-yJoystick;// + camera.transform.up*-yJoystick;
				Vector3 force = MovementVector*twoDspeed;//*Time.fixedDeltaTime;
				rigidbody.AddForce(force);
			}
			
		}


	
	
	}




}
