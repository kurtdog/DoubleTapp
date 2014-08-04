using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipController : MonoBehaviour {

	public Camera camera;
	public GameObject Projectile;
	public GameObject ShotPosition;
	public GameObject ShooterShip;
	public Transform target;
	public float acceleration;
	//public float maxSpeed;
	public float currentSpeed;
	public float fireRate;
	public float turnSpeed;
	public float twoDspeed;
	public float rotationSlow;
	public float rotationSpeed3D;
	public float rotationSpeed2D;
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
	private bool moving;

	private List<GameObject> projectiles;
	private float shotTimer;
	int invX;
	int invY;
	CameraScript cameraScript;
	// Use this for initialization
	void Start () {
		lockedOn = false;
		cameraScript = camera.GetComponent<CameraScript>();
		projectiles = new List<GameObject>();

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

		shotTimer += Time.fixedDeltaTime;
		//LockRotation();
		GetKeyInput(); // take action on button press events
		HandleControls();

		if(lockedOn)
		{
			if(target != null) // if the target exists
			{
				this.transform.LookAt(target.transform.position); // lookAtit
			}
		}

	}

	//TODO: get this working
	//not sure if I want to implement this. Maybe have it toggle-able
	void FollowTarget() // if we're locked on, we want to follow our target, i.e. stay within a certain distance of it. When the target moves, we do too
	{	
		Vector3 offset = target.position - cameraScript.currentView.transform.position;
		if(cameraScript.currentView.GetComponent<ViewPointScript>().viewPoint == Viewpoint.thirdPerson)
		{
			transform.position = target.position - (this.transform.rotation * offset);
		}
		else{
			transform.position = cameraScript.currentView.transform.position;
		}
		
	}

	//Button Press Events
	void GetKeyInput()
	{
		/*
		if(Input.GetButtonDown("Fire1") && shotTimer > fireRate)
		{
			Shoot();
		}
		*/
		//Input.GetAxis("Thrust");
		// < 0 is Left Trigger
		// > 0 is Right Trigger

		if(Input.GetButtonDown("LockOn"))
		{
			lockedOn = !lockedOn;
		}


		if(Input.GetAxis("Thrust") > .5f)//rightTrigger
		{
			this.rigidbody.AddForce(this.transform.forward*acceleration*Input.GetAxis("Thrust"));

		}
		//Debug.Log("fire: " + Input.GetAxis("Fire1"));
		if(Input.GetAxis("Fire1") > .5f && shotTimer > fireRate)//leftTrigger
		{
			Shoot();
		}
		//iff pressing back, and thrust, move backwards
		if(Input.GetButton("Thrust") && xJoystick < 0)
		{
			this.rigidbody.AddForce(-this.transform.forward*acceleration);
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
	void Shoot()
	{
		//Debug.Log("shooting");
		shotTimer = 0;
		
		GameObject bullet = Instantiate(Projectile, ShotPosition.transform.position,this.transform.rotation) as GameObject;
		
		
		//Debug.Log("speed: " + bullet.GetComponent<Projectile>().speed);
		//Debug.Log("forward: " + transform.forward);
		
		Vector3 f = -ShooterShip.transform.right*(bullet.GetComponent<Projectile>().speed +Mathf.Abs(this.rigidbody.velocity.magnitude));
		//bullet.GetComponent<Projectile>().force = f;
		//Debug.Log("adding force f: " + f);
		bullet.rigidbody.AddForce(f);
		//bullet.rigidbody.velocity = f;
		projectiles.Add(bullet);
	}
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
		xJoystick = Input.GetAxis("Horizontal1");
		yJoystick = Input.GetAxis("Vertical1");
		xJoystick2 = Input.GetAxis("Horizontal2");
		moving = false;
		
		currentSpeed = rigidbody.velocity.magnitude; // view the velocity in the inspector
		Vector3 force = new Vector3(0,0,0);
		// if we're giving input

		//rotate ship
		//in third person View, fly around, rotate the ship, move freely in 3D space
		//flip
		if(Mathf.Abs(xJoystick) > joystickThreshold) 
		{
			//rigidbody.AddForce(force);
			
			Vector3 torqueVector = Camera.main.transform.up*invX*xJoystick*rotationSpeed3D;
			rigidbody.AddTorque(torqueVector);
			moving = true;

			//torqueVector = this.transform.forward*xJoystick*rotationSpeed3D; // barrel roll
			//rigidbody.AddTorque(torqueVector);
			
		}
		// look left-right
		if(Mathf.Abs(yJoystick) > joystickThreshold)// 
		{
			Vector3 torqueVector = Camera.main.transform.right*yJoystick*invY*rotationSpeed3D; 
			rigidbody.AddTorque(torqueVector);
			moving = true;
		}


		//otherwise
		if(!moving)
		{
			//slow down the ship
			SlowDown();
		}
	}

	void twoDMovement()
	{
		//Debug.Log("2D Movement");
		xJoystick = Input.GetAxis("Horizontal1");
		yJoystick = Input.GetAxis("Vertical1");
		//xJoystick2 = Input.GetAxis("Horizontal2");
		yJoystick2 = Input.GetAxis("Vertical2");
		//Debug.Log("yjoystick2 " + yJoystick2);
		moving = false;

		Viewpoint currentViewpoint = cameraScript.currentView.GetComponent<ViewPointScript>().viewPoint;
		//third person controls
		if( currentViewpoint == Viewpoint.thirdPerson)
		{
			if(Mathf.Abs(xJoystick) > joystickThreshold || Mathf.Abs(yJoystick) > joystickThreshold ) 
			{
				//float distance = Vector3.Distance(target.transform.position,ShooterShip.transform.position);
				//Debug.Log("distance: " + distance);
				//use L-Joystick to move in a circle around your target
				float xRotation = xJoystick*rotationSpeed2D;//*(100.0f/distance); // rotate slower as you move away, and faster as you move close
				float yRotation = yJoystick*rotationSpeed2D;//*(100.0f/distance);
				transform.RotateAround(target.transform.position,Camera.main.transform.up,-xRotation);
				transform.RotateAround(target.transform.position,Camera.main.transform.right,-yRotation);
				moving = true;
			}
			if(Mathf.Abs(yJoystick2) > joystickThreshold)
			{
				//use R-Joystick to move in and out
				float force = acceleration*-yJoystick2;
				Debug.Log("force: " + force);
				this.rigidbody.AddForce(this.transform.forward*force);
				moving = true;
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
				moving = true;

			}
			if(Mathf.Abs(yJoystick) > joystickThreshold ) 
			{
				//rotate for yJoystick
				float yRotation = yJoystick*rotationSpeed2D;
				Debug.Log("yRotation: " + yRotation);
				transform.RotateAround(target.transform.position,Camera.main.transform.forward,yRotation);
				
				moving = true;
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
				moving = true;
				
			}
			if(Mathf.Abs(yJoystick) > joystickThreshold ) 
			{
				//rotate for yJoystick
				float yRotation = yJoystick*rotationSpeed2D;
				//Debug.Log("yRotation: " + yRotation);
				transform.RotateAround(target.transform.position,Camera.main.transform.forward,-yRotation);
				
				moving = true;
			}
			
		}
		else if(currentViewpoint == Viewpoint.top){
			if(Mathf.Abs(xJoystick) > joystickThreshold) 
			{
				//instead of case based movement we want to always move the ship based on the camera, right, left, up down, it's always from the camera's perspective
				//rotate for xJoystick
				float xRotation = xJoystick*rotationSpeed2D;
				//Debug.Log("xRotation: " + xRotation);
				transform.RotateAround(target.transform.position,Camera.main.transform.forward,xRotation);
				moving = true;
			}
			if(Mathf.Abs(yJoystick) > joystickThreshold ) 
			{
				//add force for yJoystick
				Vector3 MovementVector = camera.transform.up*-yJoystick;// + camera.transform.up*-yJoystick;
				Vector3 force = MovementVector*twoDspeed;//*Time.fixedDeltaTime;
				rigidbody.AddForce(force);
				moving = true;
			}
			
		}

		if(!moving)
		{
			SlowDown();
		}
	
	
	}

	void SlowDown()
	{
		if(rigidbody.velocity.magnitude > .1f)
		{
			rigidbody.AddForce(-rigidbody.velocity*acceleration*Time.fixedDeltaTime);
			//Debug.Log(shipType.ToString() +" slowing down");
		}

		if(rigidbody.angularVelocity.magnitude > .1f)
		{
			rigidbody.AddTorque(-rigidbody.angularVelocity*rotationSlow*Time.fixedDeltaTime);
			//Debug.Log("adding torque:");
		}
	}


}
