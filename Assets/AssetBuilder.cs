using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetBuilder : MonoBehaviour {

	//public GameObject AsteroidBelt;
	//public GameObject AsteroidFeild;
	public int spawnsPerUpdate;

	public List<Generator> masterQueue = new List<Generator>();



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
				Pop();
			}
		}

	}

	//Generator scripts can push items into our queue
	public void Push(Generator g)
	{
		masterQueue.Add(g);
	}

	private void Pop()
	{

		masterQueue[0].Pop(); //get the Generator.Pop method, and use it. This method is overridden by each generatorClass, and spawns an object for that generator
		masterQueue.Remove(masterQueue[0]);

		
	}
}
