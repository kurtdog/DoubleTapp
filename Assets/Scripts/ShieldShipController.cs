using UnityEngine;
using System.Collections;

public class ShieldShipController : MonoBehaviour {

	public GameObject ShooterShip;
	public GameObject LockWheel;
	public GameObject camera;
	public float rotationSpeed;
	public float angle;
	ShipController shipController;
	MusicManager lockWheel;
	CameraScript cameraScript;

	public float xJoystick;
	public float yJoystick;
	private float radius;

	// Use this for initialization
	void Start () {
	
		shipController = ShooterShip.GetComponent<ShipController>();
        lockWheel = LockWheel.GetComponent<MusicManager>();
		cameraScript = camera.GetComponent<CameraScript>();
		angle = 0;
		radius = lockWheel.radius;
	}
	
	// Update is called once per frame
	void Update () {
		HandleInput();
	}


	void HandleInput()
	{

		if(Input.GetButton("MoveShieldLeft"))
		{
			angle += rotationSpeed;
		}
		if(Input.GetButton("MoveShieldRight"))
		{
			angle -= rotationSpeed;
		}
	
		float deg = Mathf.Deg2Rad * angle;
		
		Vector3 position = new Vector3(0,0,0);
		
		//Make this based on the camera
		
		if((cameraScript.currentView.GetComponent<ViewPointScript>().viewPoint == Viewpoint.sideL) || (cameraScript.currentView.GetComponent<ViewPointScript>().viewPoint == Viewpoint.sideR))
		{
			position.y = Mathf.Sin (deg) * radius ;
			position.z = Mathf.Cos (deg) * radius;
			position.x = 0;
		}
		else{
			position.x = Mathf.Cos (deg) * radius;
			position.y = 0 ;
			position.z = Mathf.Sin (deg) * radius;
		}

		//Debug.Log("Shooter Pos: " + ShooterShip.transform.position);
		//Debug.Log("Shield Pos: " + position);
		this.transform.localPosition = position;
		//this.transform.Rotate(lockWheel.transform.up,xJoystick*rotationSpeed*Time.fixedDeltaTime);

	}

	void JoystickControls()
	{

		

	}


}
