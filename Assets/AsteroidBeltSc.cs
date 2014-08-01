using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidBeltSc : MonoBehaviour {
	
	public List<GameObject> asteroids;
	public List<GameObject> spawnedObjects;
	public int Spawn;
	public float minSize;
	public float maxSize;

	public int radiusInnerRing;
	public int radiusOuterRing;
	public Vector3 upAxis;

	public float torqueMin;
	public float torqueMax;

	public float BeltSpinSpeed;

	// Use this for initialization
	void Start () {
		
		spawnedObjects = new List<GameObject>();

		SpawnAsteroids();
	}
	
	// Update is called once per frame
	void Update () {

		this.transform.RotateAround(this.transform.position,this.transform.forward,BeltSpinSpeed*Time.fixedDeltaTime);
		
	}
	
	void SpawnAsteroids()
	{
		Random random = new Random();
		for(int i = 0; i < Spawn; i++)
		{
			float distance = Random.Range(radiusInnerRing,radiusOuterRing); // random distance between the inner and outer radius
			//pick a random vector inside the unit circle, multiply it by this distance
			Vector3 randomLocation = Random.insideUnitCircle.normalized*distance;

			//Quaternion randomRotation = Random.rotation;

			GameObject ast = Instantiate(GetRandomAsteroid(),randomLocation,this.transform.rotation) as GameObject;
			ast.rigidbody.AddTorque(GetRandomTorque());
			ast.transform.parent = this.transform;
			float size = Random.Range(minSize,maxSize);
			
			ast.transform.localScale += new Vector3(size,size,size);
			
			spawnedObjects.Add(ast);
		}
		
		this.transform.LookAt(upAxis);
	}

	Vector3 GetRandomTorque()
	{
		float torqueX = Random.Range(torqueMin,torqueMax);
		float torqueY = Random.Range(torqueMin,torqueMax);
		float torqueZ = Random.Range(torqueMin,torqueMax);

		return new Vector3(torqueX,torqueY,torqueZ);
	}


	GameObject GetRandomAsteroid()
	{
		int index = Random.Range(0,asteroids.Count);
		
		return asteroids[index];
	}

}
