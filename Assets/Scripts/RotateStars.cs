using UnityEngine;
using System.Collections;

public class RotateStars : MonoBehaviour {

	public bool clockwise;
	public int speed;

	// Update is called once per frame
	void FixedUpdate()
	{
		RotateChildren();
	}

	void RotateChildren()
	{

		int i = -1;
		if(clockwise)
		{
			i = 1;
		}

		foreach( Transform tf in GetComponentsInChildren<Transform>())
		{
			tf.RotateAround(this.transform.position,this.transform.up,i*speed*Time.fixedDeltaTime);

		}

	}
}
