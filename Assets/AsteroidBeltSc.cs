using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidBeltSc : Generator {
	
	public List<GameObject> asteroids;
	//public List<GameObject> spawnedObjects;

	public float minSize;
	public float maxSize;

	public int radiusInnerRing;
	public int radiusOuterRing;
	//public int queue;
	//public int spawnsPerUpdate;

	public Vector3 upAxis;

	public float torqueMin;
	public float torqueMax;

	public float BeltSpinSpeed;
	
	//private Vector3 startPposl;

	// Use this for initialization

	
	// Update is called once per frame
	void  Update () {
		ParentUpdate();
		this.transform.RotateAround(this.transform.position,this.transform.forward,BeltSpinSpeed*Time.fixedDeltaTime);
	}

	public override void Pop()
	{
		SpawnAsteroid();
	}



	//TODO: needs fixing
	void SpawnAsteroid()
	{
		Random random = new Random();
		//Debug.Log("Spawning Ast");
		float distance = Random.Range(radiusInnerRing,radiusOuterRing); // random distance between the inner and outer radius
		//pick a random vector inside the unit circle, multiply it by this distance
		Vector3 randomLocation = Random.insideUnitCircle.normalized*distance;
		//Vector3 randomLocation = Random.insideUnitCircle*this.transform.forward*distance;

		//Quaternion randomRotation = Random.rotation;

		GameObject ast = Instantiate(GetRandomAsteroid(),randomLocation,this.transform.rotation) as GameObject;
		ast.rigidbody.AddTorque(GetRandomTorque());
		ast.transform.parent = this.transform;
		float size = Random.Range(minSize,maxSize);
		
		ast.transform.localScale += new Vector3(size,size,size);
		
		spawnedObjects.Add(ast);


		
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
