using UnityEngine;
using System.Collections;

public class LaserScript : MonoBehaviour 
{
	LineRenderer line;
	public int length;
	public float noise;
	
	void Start () 
	{
		line = gameObject.GetComponent<LineRenderer>();
		line.enabled = true; //false

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
		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;
		
		line.SetPosition(0, ray.origin);
		
		if(Physics.Raycast(ray, out hit, length))
		{
			line.SetPosition(1, hit.point);
			if(hit.rigidbody)
			{
				//hit.rigidbody.AddForceAtPosition(transform.forward* 10, hit.point);
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