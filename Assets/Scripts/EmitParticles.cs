using UnityEngine;
using System.Collections;

public class EmitParticles : MonoBehaviour {


	private ParticleSystem particleSystem;
	Vector3 oldPosition;
	public float minDistance;// if we move more than this, we emit particles

	// Use this for initialization
	void Start () {
		particleSystem = this.GetComponent<ParticleSystem> ();
		particleSystem.playOnAwake = false;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (this.transform.position != oldPosition && Vector3.Distance(this.transform.position,oldPosition) > minDistance) 
		{
			particleSystem.Emit(1);


		} 

		oldPosition = this.transform.position;

	}
}
