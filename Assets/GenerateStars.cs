using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateStars : MonoBehaviour {

	public List<GameObject> CloudObjects;

	public GameObject GenerationObject;
	//PlayerController playerController;

	public float generationRate;
	//the min and max distance away from the player to generate clouds
	public int minDistance;
	public int maxDistance;



	private float timer = 0;
	// Use this for initialization
	void Start () {

	}
	


	void FixedUpdate()
	{
		timer += Time.fixedDeltaTime;
	}
	// Update is called once per frame
	void Update () {

		MakeClouds();
	}

	private void MakeClouds()
	{
		Vector2 randomCircle = Random.insideUnitCircle*Random.Range(0,minDistance); // an item placed at a random location in the unit circle, scaled by by Random.Range(0,mindistance)

		if(timer > generationRate)
		{
			Vector3 position = GenerationObject.transform.position + new Vector3(randomCircle.x,0,randomCircle.y) + new Vector3(0,Random.Range(-minDistance,-maxDistance),0);
		 	//make a cloud object based off a random cloud in our CloudObjects list.
			GameObject newCloud = Instantiate(CloudObjects[Random.Range(0,CloudObjects.Count-1)],position,this.transform.rotation) as GameObject;
			timer = 0;
		}
	}


}
