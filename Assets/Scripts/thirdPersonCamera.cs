using UnityEngine;
using System.Collections;

public class thirdPersonCamera : MonoBehaviour {

	/*Defaults
	 * xLerp = 2.5
	 * yLerp = 5;
	 * xLerp = 5;
	 * HyperLerp = 10;
	 * xAxisLerp = -5;
	 * yAxis Lerp = .25
	 * zAxis Lerp = 1;
	 * AngleThresh = 30;
	 * */

	public GameObject target;
	public Vector3 offset; // initial offset from the player
	public float xLerpSpeed;
	public float yLerpSpeed;
	public float zLerpSpeed;
	public float hyperLerpSpeed;

	public float xAxisLerpSpeed;
	public float yAxisLerpSpeed;
	public float zAxisLerpSpeed;
	//used to keep angles from being 0, it causes errors with the lerp system.
	public float angleThreshold;
	//public int startLerpThreshold; // distance at which we follow the ship directly;
	bool lerp = true;

	float lastVelocity;
	Vector3 lastOffset;
	float velocityX;
	float velocityY;
	float velocityZ;

	float xOffset;
	float yOffset;
	float zOffset;
	

	float initialAngleX;

	void Start() {
		offset = this.transform.position - target.transform.position;
		Vector3 desiredPosition = target.transform.position +target.transform.right*offset.x +target.transform.up*offset.y + target.transform.forward*offset.z;
		xOffset = desiredPosition.x;
		yOffset = desiredPosition.y;
		zOffset = desiredPosition.z;

		initialAngleX = this.transform.eulerAngles.x;
	}
	
	void LateUpdate() {
		FollowPlayer ();

	}

	void FollowPlayer()
	{
		Vector3 desiredPosition = target.transform.position +target.transform.right*offset.x +target.transform.up*offset.y + target.transform.forward*offset.z;
		//this.transform.position = target.transform.position + offset;

		if(Input.GetButton("HyperThrust"))
		{
			xOffset = Mathf.Lerp (xOffset,desiredPosition.x, hyperLerpSpeed * Time.fixedDeltaTime);
			yOffset = Mathf.Lerp (yOffset, desiredPosition.y, hyperLerpSpeed * Time.fixedDeltaTime);
			zOffset = Mathf.Lerp (zOffset, desiredPosition.z, hyperLerpSpeed * Time.fixedDeltaTime);
		}
		else
		{
			xOffset = Mathf.Lerp (xOffset,desiredPosition.x, xLerpSpeed * Time.fixedDeltaTime);
			yOffset = Mathf.Lerp (yOffset, desiredPosition.y, yLerpSpeed * Time.fixedDeltaTime);
			zOffset = Mathf.Lerp (zOffset, desiredPosition.z, zLerpSpeed * Time.fixedDeltaTime);
		}

		this.transform.position = new Vector3(xOffset,yOffset,zOffset);//


		LerpAngle ();
	}

	void LerpAngle()
	{
		// get the lookatVector
		Vector3 lookAt = (target.transform.position - this.transform.position).normalized; 
		//lerp the x,y, and z components of this.transform.forward to match the lookAt x, y, and z
		float xAngle = Mathf.SmoothDampAngle (this.transform.forward.x, lookAt.x, ref velocityX, xAxisLerpSpeed * Time.fixedDeltaTime);
		float yAngle = Mathf.SmoothDampAngle (this.transform.forward.y, lookAt.y, ref velocityY, yAxisLerpSpeed * Time.fixedDeltaTime);
		float zAngle = Mathf.SmoothDampAngle (this.transform.forward.z, lookAt.z, ref velocityZ, zAxisLerpSpeed * Time.fixedDeltaTime);
		this.transform.forward = new Vector3 (xAngle, yAngle, zAngle);


		//Rotate around the z axis to keep the camera.up close to the target.up
		float deltaZ = (target.transform.eulerAngles.z - this.transform.eulerAngles.z);
		float desiredAngleZ = target.transform.eulerAngles.z;
		float currentZ = Mathf.Lerp (this.transform.eulerAngles.z, desiredAngleZ, zAxisLerpSpeed);
		//Debug.Log ("desiredZ: "  + desiredAngleZ + " currentZ: " + currentZ  + " deltaZ: " + deltaZ);
		this.transform.rotation = Quaternion.Euler (this.transform.eulerAngles.x, this.transform.eulerAngles.y, currentZ);//Quaternion.Euler(currentX, currentY, currentZ);


	}


}















