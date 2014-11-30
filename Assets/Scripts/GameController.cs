using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * This class is responsible for tracking player status, and generating the game accourdingly
 * */
public class GameController : MonoBehaviour {

    public GameObject Ship;
    //public GameObject Ship;
    ShipController shipController;
    public GameObject AssetBuilder;
    AssetBuilder assetBuilder;
    //TOOD: have these objects fall in to ceratin categories: stars, fields, enemies, etc.
    public List<GameObject> spaceObjectsToGenerate; // generator classes, and space objects like starts, nebulas, etc.
   // public List<GameObject> enemiesToGenerate; //TODO: set difficulty levels for each enemy

    public int newObjectDistanceMin; // how far away to generate new objects
    public int newObjectDistanceMax;
    public int objectsToGenerate;
   // public int minDistFromObjects; // minimum distance we have to be from objects in order to spawn something new
    public List<GameObject> generatedObjects;
    

	// Use this for initialization
	void Start () {
        generatedObjects = new List<GameObject>();
        assetBuilder = AssetBuilder.GetComponent<AssetBuilder>();
        shipController = Ship.GetComponent<ShipController>();
	}
	
	// Update is called once per frame
	void Update () {
        //TODO: only do this every x updates? or if we're y distance away from all of the existing objects
        Debug.Log("somethingOnScreen: " + somethingOnScreen());
        Debug.Log("closeToSomething: " + closeToSomething());
        bool spawnSomething = assetBuilder.masterQueue.Count == 0  && !somethingOnScreen() && !closeToSomething();
        Debug.Log("scr,close,spawn: (" + somethingOnScreen() + "," + closeToSomething() + "," + spawnSomething + ")");
        if (spawnSomething)// && generatedObjects.Count < objectsToGenerate) // if there is nothing to generate
        {
            int index = Random.Range(0, spaceObjectsToGenerate.Count);
            GameObject newObject = Instantiate( spaceObjectsToGenerate[index], getRandomPositionOnScreen(), this.transform.rotation) as GameObject;
            generatedObjects.Add(newObject);
            if(newObject.GetComponent<Generator>() != null)
            {
                newObject.GetComponent<Generator>().assetBuilder = this.assetBuilder;
            }
            if (newObject.GetComponent<MeteorShower>() != null)
            {
                newObject.GetComponent<MeteorShower>().Shooter = Ship;
            }
        }
        

	}

    //generate a new object newObjectDistance away that apears on teh screen, with a random x and y offset.
    Vector3 getRandomPositionOnScreen()
    {
        //TOOD: add offsets, use trig.
        //tan(45) = opposite/adjacent
        //adjacent = newObjectDistance;
        //opposite = tan(45)*newObjectDistance
        //x = opposite, or y = opposite
        //Or maybe just have a public var for min and max x and y offset.
        Vector3 position = Ship.transform.position + Ship.transform.forward * Random.Range(newObjectDistanceMin, newObjectDistanceMax);

        return position;
    }

    bool closeToSomething()
    {
        foreach (GameObject generatedObject in generatedObjects)
        {
            if (Vector3.Distance(Ship.transform.position, generatedObject.transform.position) < newObjectDistanceMax)
            {
                return true;
            }
        }
        return false;

    }

    bool somethingOnScreen()
    {
        foreach(GameObject generatedObject in generatedObjects)
        {
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(generatedObject.transform.position);
            if(screenPoint.x < 0 || screenPoint.x > Screen.width || screenPoint.y < 0 || screenPoint.y > Screen.height)
            {
                return false;
            }
            /*
            if( generatedObject.renderer.isVisible)
            {
                return true;
            }
             * */
        }
        return false;
    }
}
