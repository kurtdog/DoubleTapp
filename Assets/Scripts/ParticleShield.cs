using UnityEngine;
using System.Collections;

public class ParticleShield : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		Particle[] particles = particleEmitter.particles;
		int i = 0;
		while (i < particles.Length) {
			float yPosition = Mathf.Sin(Time.time) * Time.deltaTime;
			particles[i].position += new Vector3(0, yPosition, 0);
			particles[i].color = Color.red;
			particles[i].size = Mathf.Sin(Time.time) * 0.2F;
			i++;
		}
		particleEmitter.particles = particles;
	}
}
