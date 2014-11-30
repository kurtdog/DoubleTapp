using UnityEngine;
using System.Collections;

public class WarpPortal : MonoBehaviour {

	public int warpSpeed;
	public int lifetime;
	public GameObject warpTrail;

	private float timer = 0;
	// Use this for initialization
	void Start () {
		timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
		timer += Time.fixedDeltaTime;
		if(timer > lifetime)
		{
			Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.rigidbody != null)
		{
			GameObject warptrail = Instantiate(warpTrail,col.transform.position,col.transform.rotation) as GameObject;
			warptrail.transform.parent = col.transform;
			col.gameObject.rigidbody.AddForce (this.transform.forward.normalized * warpSpeed);
		}
	}
}
