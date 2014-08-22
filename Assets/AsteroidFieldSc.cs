using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Generate asteroids in a sphere.
public class AsteroidFieldSc : Generator {


	public List<GameObject> asteroids;

	//public int Spawn;
	public int radius;
	public float minSize;
	public float maxSize;

	//torqueMin
	//torqueMax


	public override void Pop()
	{

		SpawnAsteroid();

	}

	void SpawnAsteroid()
	{
		Random random = new Random();


			Vector3 randomLocation = this.transform.position + Random.insideUnitSphere*radius;
			Quaternion randomRotation = Random.rotation;
			GameObject ast = Instantiate(GetRandomAsteroid(),randomLocation,randomRotation) as GameObject;
			ast.transform.parent = this.transform;
			float size = Random.Range(minSize,maxSize);

			ast.transform.localScale += new Vector3(size,size,size);

			spawnedObjects.Add(ast);




	}


	GameObject GetRandomAsteroid()
	{
		int index = Random.Range(0,asteroids.Count);

		return asteroids[index];
	}


}
