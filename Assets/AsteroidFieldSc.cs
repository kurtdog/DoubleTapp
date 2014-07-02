using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Generate asteroids in a sphere.
public class AsteroidFieldSc : MonoBehaviour {


	public GameObject Asteroid;
	public List<GameObject> asteroids;
	public int Spawn;
	public int radius;
	public float minSize;
	public float maxSize;

	//torqueMin
	//torqueMax

	// Use this for initialization
	void Start () {
	
		asteroids = new List<GameObject>();
		SpawnAsteroids();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SpawnAsteroids()
	{
		Random random = new Random();
		for(int i = 0; i < Spawn; i++)
		{

			Vector3 randomLocation = Random.insideUnitSphere*radius;
			Quaternion randomRotation = Random.rotation;
			GameObject ast = Instantiate(Asteroid,randomLocation,randomRotation) as GameObject;
			ast.transform.parent = this.transform;
			float size = Random.Range(minSize,maxSize);

			ast.transform.localScale += new Vector3(size,size,size);

			asteroids.Add(ast);
		}


	}


}
