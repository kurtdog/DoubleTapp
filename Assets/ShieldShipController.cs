using UnityEngine;
using System.Collections;

public class ShieldShipController : ShipController {

	public GameObject ShooterShip;
	public float distFromShooter;
	public float adjustmentSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		CallUpdateEvents();
	}

	//align this ship to the same z distance away from the camera's perspective
	void AlignShips()
	{
		//Lerp Position
		Vector3 OptimalShieldPosition = ShooterShip.transform.position + (ShooterShip.transform.forward*this.transform.lossyScale.x*distFromShooter);
		if(this.transform.position.z != OptimalShieldPosition.z)
		{
			Debug.Log("AlligningShips");
			this.transform.position = Vector3.Lerp(this.transform.position,OptimalShieldPosition,Time.fixedDeltaTime*adjustmentSpeed);
		}

		//Lerp Rotation

	}
}
