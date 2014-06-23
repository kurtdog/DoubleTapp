using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipController : MonoBehaviour {

	public Camera camera;
	public GameObject Projectile;
	public GameObject ShotPosition;
	public float acceleration;
	public float maxSpeed;
	public float currentSpeed;
	public float fireRate;
	public float turnSpeed;
	public float rotationSlow;
	public ShipType shipType;
	public enum ShipType{Shooter,Shield};
	
	public float xJoystick;
	public float yJoystick;
	
	private List<GameObject> projectiles;
	private float shotTimer;

	CameraScript cameraScript;
	// Use this for initialization
	void Start () {
		cameraScript = camera.GetComponent<CameraScript>();
		projectiles = new List<GameObject>();
	}
	
	// Update is called once per frame
	public void Update () {

		CallUpdateEvents();

	}

	public void CallUpdateEvents()
	{
		shotTimer += Time.fixedDeltaTime;
		//LockRotation();
		GetKeyInput(); // take action on button press events
		GetAxisInput(); //get the axis information, joystick controls
		HandleControls();

	}

	void LockRotation()
	{
		//lock the rotation of this object. we don

	}

	//Button Press Events
	void GetKeyInput()
	{
		if(Input.GetButtonDown("Fire1") && shotTimer > fireRate)
		{
			Debug.Log("shooting");
			shotTimer = 0;
			if(shipType == ShipType.Shooter)
			{
				GameObject bullet = Instantiate(Projectile, ShotPosition.transform.position,this.transform.rotation) as GameObject;

				
				Debug.Log("speed: " + bullet.GetComponent<Projectile>().speed);
				Debug.Log("forward: " + transform.forward);
				
				Vector3 f = this.transform.forward*bullet.GetComponent<Projectile>().speed;
				//bullet.GetComponent<Projectile>().force = f;
				Debug.Log("adding force f: " + f);
				bullet.rigidbody.AddForce(f);
				projectiles.Add(bullet);
			}
		}
		if(Input.GetButton("Thrust"))
		{
			this.rigidbody.AddForce(this.transform.forward*acceleration);

		}
		
	}

	void GetAxisInput()
	{
		//depending on ship type, get the joystick input for this ship
		if(shipType == ShipType.Shooter)
		{
			xJoystick = Input.GetAxis("Horizontal1");
			yJoystick = Input.GetAxis("Vertical1");
		}
		else if(shipType == ShipType.Shield)
		{
			xJoystick = Input.GetAxis("Horizontal2");
			yJoystick = Input.GetAxis("Vertical2");
		}
	}

	//restrict movement in the Z axis
	//controls act as an arcade style 2D game in this mode
	void HandleControls()
	{
		currentSpeed = rigidbody.velocity.magnitude; // view the velocity in the inspector
		Vector3 force = new Vector3(0,0,0);

		//instead of case based movement we want to always move the ship based on the camera, right, left, up down, it's always from the camera's perspective
		Vector3 MovementVector = camera.transform.right*xJoystick + camera.transform.up*yJoystick;


		force = MovementVector*acceleration*Time.fixedDeltaTime;
		
		// if we're giving input
		if(xJoystick != 0.0f || yJoystick != 0.0f) 
		{
			//addforce
			if(currentSpeed < maxSpeed)
			{
				rigidbody.AddForce(force);
				//rigidbody.AddTorque(new Vector3(xJoystick*turnSpeed,0,0));
				// W key or the up arrow to turn upwards, S or the down arrow to turn downwards.
				//rigidbody.AddTorque (new Vector3(0,yJoystick*turnSpeed,0));
				// A or left arrow to turn left, D or right arrow to turn right. 
				//float angle = Vector2.Angle(movingDirection,new Vector2(transform.forward.x,transform.forward.y));
				///transform.Rotate(new Vector3(0,0,1), angle);
				//Debug.Log(shipType.ToString() +" force: " + force);
			}
			//rotate ship
		}
		//otherwise
		else{
			//slow down the ship
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
			Debug.Log("adding torque:");
		}
	}


}
