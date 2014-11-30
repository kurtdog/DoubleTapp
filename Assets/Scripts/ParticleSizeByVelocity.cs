using UnityEngine;
using System.Collections;

public class ParticleSizeByVelocity : MonoBehaviour {

    public GameObject Ship;
    ShipController shipController;

    ParticleSystem particleSystem;

    float originalStartSize;
	// Use this for initialization
	void Start () {

        shipController = Ship.GetComponent<ShipController>();
        particleSystem = this.GetComponent<ParticleSystem>();
        originalStartSize = particleSystem.startSize;
	}
	
	// Update is called once per frame
	void Update () {

        float size = Mathf.Min(1,((Ship.rigidbody.velocity.magnitude*2) / shipController.threeDmovement.speed));
        //Debug.Log(size);
        particleSystem.startSize = originalStartSize*size;
        
	}
}
