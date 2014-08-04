using UnityEngine;
using System.Collections;

public class LaserScript : MonoBehaviour 
{
	LineRenderer line;
	public int length;
	public float noise;
	private Material originalMat;
	public Material highLightMat;

	//public Transform target; // moved to ship Controller
	public GameObject ShooterShip;
	ShipController shipController;

	void Start () 
	{
		line = gameObject.GetComponent<LineRenderer>();
		line.enabled = true; //false
		shipController = this.GetComponent<ShipController>();

	}
	void Update () 
	{
		FireLaser2();
		/*
		if(Input.GetButtonDown("Fire1"))
		{
			StopCoroutine("FireLaser");
			StartCoroutine("FireLaser");
		}
		*/
	}

	public void FireLaser2()
	{
		Ray ray = new Ray(ShooterShip.transform.position, transform.forward);
		RaycastHit hit;
		
		line.SetPosition(0, ShooterShip.transform.position); //ray.origin
		
		if(Physics.Raycast(ray, out hit, length))
		{
			line.SetPosition(1, hit.point);
			if(!hit.collider.isTrigger && !shipController.lockedOn) //if we aren't locked on, look for a new target
			{
				//hit.rigidbody.AddForceAtPosition(transform.forward* 10, hit.point);
				if(shipController.target != null)
				{
					shipController.target.renderer.material = originalMat; // Unhighlight the last target
				}
				shipController.target = hit.transform; // target = new target
				originalMat = shipController.target.renderer.material; // record the items original material
				highLightMat.mainTexture = originalMat.mainTexture; // copy the texture
				shipController.target.renderer.material = highLightMat; //highlight the new target //color higlight

			}
		}
		else
			line.SetPosition(1, ray.GetPoint(100));
		
		//yield return null;
	}
	


	IEnumerator FireLaser()
	{
		line.enabled = true;
		
		while(Input.GetButton("Fire1"))
		{
			Ray ray = new Ray(transform.position, transform.forward);
			RaycastHit hit;
			
			line.SetPosition(0, ray.origin);
			
			if(Physics.Raycast(ray, out hit, 100))
			{
				line.SetPosition(1, hit.point);
				if(hit.rigidbody)
				{
					hit.rigidbody.AddForceAtPosition(transform.forward* 10, hit.point);
				}
			}
			else
				line.SetPosition(1, ray.GetPoint(100));
			
			yield return null;
		}
		
		line.enabled = false;
	}
}