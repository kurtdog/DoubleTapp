using UnityEngine;
using System.Collections;

public class ParticleCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnParticleCollision(GameObject other) {
		Rigidbody body = other.rigidbody;
		//Debug.Log("Collision");
		/*
		if (body) {
			Vector3 direction = other.transform.position - transform.position;
			direction = direction.normalized;
			body.AddForce(direction * 5);
		}
		*/
	}
}
