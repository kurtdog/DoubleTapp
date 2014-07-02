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
	public float twoDspeed;
	public float rotationSlow;
	public float rotationSpeed;

	//public ShipType shipType;
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
			//Debug.Log("shooting");
			shotTimer = 0;
		
			GameObject bullet = Instantiate(Projectile, ShotPosition.transform.position,this.transform.rotation) as GameObject;

			
			//Debug.Log("speed: " + bullet.GetComponent<Projectile>().speed);
			//Debug.Log("forward: " + transform.forward);
			
			Vector3 f = this.transform.forward*(bullet.GetComponent<Projectile>().speed +  this.rigidbody.velocity.magnitude);
			//bullet.GetComponent<Projectile>().force = f;
			//Debug.Log("adding force f: " + f);
			bullet.rigidbody.AddForce(f);
			projectiles.Add(bullet);

		}
		//Input.GetAxis("Thrust");
		// < 0 is Left Trigger
		// > 0 is Right Trigger
		if(Input.GetAxis("Thrust") < 0)
		{
			this.rigidbody.AddForce(this.transform.forward*acceleration*-Input.GetAxis("Thrust"));

		}
		//iff pressing back, and thrust, move backwards
		if(Input.GetButton("Thrust") && xJoystick < 0)
		{
			this.rigidbody.AddForce(-this.transform.forward*acceleration);
		}

		
	}



	//restrict movement in the Z axis
	//controls act as an arcade style 2D game in this mode
	void HandleControls()
	{
		xJoystick = Input.GetAxis("Horizontal1");
		yJoystick = Input.GetAxis("Vertical1");


		currentSpeed = rigidbody.velocity.magnitude; // view the velocity in the inspector
		Vector3 force = new Vector3(0,0,0);

		// if we're giving input
		if(Mathf.Abs(xJoystick) > 0.4f || Mathf.Abs(yJoystick) > 0.4f ) 
		{

			//in third person View, fly around, rotate the ship, move freely in 3D space
			if(cameraScript.viewpoint == Viewpoint.thirdPerson)
			{
				//addtorque
				if(currentSpeed < maxSpeed)
				{
					//rigidbody.AddForce(force);

					Vector3 torqueVector = this.transform.right*yJoystick*rotationSpeed; //flip
					rigidbody.AddTorque(torqueVector);

					torqueVector = this.transform.forward*-xJoystick*rotationSpeed; // barrel roll
					rigidbody.AddTorque(torqueVector);

				}
			}
			//otherwise we want 2D movement
			else{
				//instead of case based movement we want to always move the ship based on the camera, right, left, up down, it's always from the camera's perspective
				Vector3 MovementVector = camera.transform.right*xJoystick + camera.transform.up*yJoystick;
				force = MovementVector*twoDspeed*Time.fixedDeltaTime;
				rigidbody.AddForce(force);

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
			//Debug.Log("adding torque:");
		}
	}


}
