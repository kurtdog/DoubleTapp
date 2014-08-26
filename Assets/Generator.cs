﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Generator : MonoBehaviour {


	public AssetBuilder assetBuilder;
	public List<GameObject> spawnedObjects;
	public int itemsToSpawn;

	public abstract void Pop(); // to be implemented by sublclasses
	//public abstract void Push();

	void Start()
	{
		spawnedObjects = new List<GameObject>();
	}

	void Update()
	{
		ParentUpdate();

	}
	// Update is called once per frame
	//this way child classes can call this class if they need to ovveride the Update Class
	public void ParentUpdate () {
        // if teh assetBuilder doesn't contain this generator, and iff we still have items to spawn
        if (!assetBuilder.masterQueue.Contains(this.gameObject) && spawnedObjects.Count < itemsToSpawn) 
		{
			assetBuilder.Push(this.gameObject);  // push this generator up to the AssetBuilder
		}
	}
}