using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SolarSystemGenerator : Generator {

	public GameObject PlanetGeneratorPrefab;
	public List<GameObject> starPrefabs;

	public int minNumPlanets;
	public int maxNumPlanets;

	public int minDistance;
	public int maxDistance;

	PlanetGenerator planetGenerator;
    PlanetNameGenerator2 planetNameGenerator;
    List<string> planetNames;
    //int planetNameIndex = 0; // index into the planet Names. //NOTE: using spawnedItems.Count instead

	// Use this for initialization
	void Start () {
        ParentStart();
		itemsToSpawn = Random.Range (minNumPlanets, maxNumPlanets);
		planetGenerator = PlanetGeneratorPrefab.GetComponent<PlanetGenerator> ();
        planetNameGenerator = this.GetComponent<PlanetNameGenerator2>();
        planetNames = planetNameGenerator.GetSolarSystemNames(itemsToSpawn); //generate a list of names to use for the planets
	}
	
	// Update is called once per frame
	void  Update () {
		ParentUpdate();
	}
	
	public override void Pop()
	{
		Random random = new Random();
		//Debug.Log("Spawning Ast");
		float distance = Random.Range(minDistance,maxDistance); // random distance between the inner and outer radius
		//pick a random vector inside the unit circle, multiply it by this distance
		//Vector3 randomLocation = Random.insideUnitSphere*distance this.transform.forward;
		//Vector3 randomLocation = this.transform.position + Random.insideUnitSphere.normalized*distance;
		Vector2 randomInUnitCircle = Random.insideUnitCircle.normalized * distance;
		Vector3 randomLocation; 

		if(spawnedObjects.Count == 0) // if it's our first thing to spawn, spawn a star
		{
			//randomLocation = this.transform.position + this.transform.right * randomInUnitCircle.x + this.transform.forward * randomInUnitCircle.y;
			randomLocation = new Vector3(0,-50000,50000);

			GameObject starPrefab = starPrefabs[Random.Range(0,starPrefabs.Count-1)]; // get a random star prefab
			GameObject star = Instantiate(starPrefab, randomLocation, this.transform.rotation) as GameObject;
            star.name = planetNames[0];
			spawnedObjects.Add(star);
		}
		else // otherwise spawn a planet
		{
			//generate planets around the star
			randomLocation = spawnedObjects[0].transform.position + this.transform.right * randomInUnitCircle.x + this.transform.forward * randomInUnitCircle.y;

			GameObject planet = planetGenerator.GeneratePlanet(randomLocation);
			//planet.name = "Planet" + spawnedObjects.Count;
            planet.name = planetNames[spawnedObjects.Count];
			spawnedObjects.Add(planet);
		}


	}
}
