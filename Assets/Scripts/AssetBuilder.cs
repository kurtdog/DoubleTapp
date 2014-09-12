using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetBuilder : MonoBehaviour {

	//public GameObject AsteroidBelt;
	//public GameObject AsteroidFeild;
	public int spawnsPerUpdate;

    public List<GameObject> masterQueue = new List<GameObject>();



	// Use this for initialization
	void Start () {
		//initialize all of our queues
		//queue += AsteroidBelt.get
	}
	
	// Update is called once per frame
	void Update () {

		for(int i = 0; i < spawnsPerUpdate; i++)
		{
			if(masterQueue.Count > 0)
			{
                //Debug.Log("Popping: " + masterQueue[0]);
				Pop();
			}
		}

	}

	//Generator scripts can push items into our queue
    public void Push(GameObject g)
	{
		masterQueue.Add(g);
	}

    //call the Pop method for the generator currently in the queue's first spot.
	private void Pop()
	{
        if(masterQueue[0].GetComponent<Generator>() != null)
        {
            Generator generator = masterQueue[0].GetComponent<Generator>();
            //Debug.Log("(" + generator.spawnedObjects.Count + "," + generator.itemsToSpawn + ")");
            if (generator.spawnedObjects.Count < generator.itemsToSpawn)
            {
               // Debug.Log("generator.Pop");
                generator.Pop(); //get the Generator.Pop method, and use it. This method is overridden by each generatorClass, and spawns an object for that generator
            }
            else
            {
                //Debug.Log("Removing: " + masterQueue[0]);
                masterQueue.Remove(masterQueue[0].gameObject);
            }
        }
	}
}
