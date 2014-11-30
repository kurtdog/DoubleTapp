using UnityEngine;
using System.Collections;

public class WarpTrail : MonoBehaviour {

	public float lifetime;

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
}
