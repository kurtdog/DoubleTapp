using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MeteorShower : Generator {

	public List<GameObject> meteors;
	public int radius;
	public float minSize;
	public float maxSize;
	public int meteorSpeed;
	public float particleSize;
	public float particleLife;
	public GameObject Shooter;

	public bool pointAtPlayer;

	// Update is called once per frame

	void Start()
	{
		if(pointAtPlayer)
		{
			this.transform.forward = Shooter.transform.position - this.transform.position;
		}
	}

	public override void Pop()
	{
		
		SpawnMeteor();
		
	}

	void  Update () {
		ParentUpdate();
		this.rigidbody.AddForce(meteorSpeed*this.transform.forward);
	}

	void SpawnMeteor()
	{
		Random random = new Random();
		
		Debug.Log("Spawning Random Meteor");
		Vector3 randomLocation = this.transform.position + Random.insideUnitSphere*radius;
		Quaternion randomRotation = Random.rotation;
		GameObject meteor = Instantiate(GetRandomMeteor(),randomLocation,randomRotation) as GameObject;
		meteor.transform.parent = this.transform;
		float size = Random.Range(minSize,maxSize);
		meteor.transform.localScale += new Vector3(size,size,size);
		meteor.GetComponent<ParticleSystem>().startSize = particleSize*size;
		meteor.GetComponent<ParticleSystem>().startLifetime = particleLife*size;
		meteor.rigidbody.AddForce(meteorSpeed*this.transform.forward);
		meteor.transform.rotation = this.transform.rotation;

		spawnedObjects.Add(meteor);

	}

	GameObject GetRandomMeteor()
	{
		int index = Random.Range(0,meteors.Count);
		
		return meteors[index];
	}
}
