using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {

	//TODO: figure out what to do. We have some stuff like asteroid fields, that have a predictable shape and size. For these we could pre-create a prefab used for guiPop;
	//but for Random objects like planets, we have to generate the guiPop with the GameObject. How does that affect how this script works?

	public List<GameObject> mapObjects; // TODO: in theory we no longer need this list, we could just use the list of guiPopups since they have a reference to their corresponging mapOjbects..
	public List<GameObject> onScreen;
	public List<GameObject> guiPopups; //TODO: scale based on size
	public int mapStartRadius;
	public int mapEndRadius;

	public bool displayMap;


	private float minDistance = 10000000000; // start really high
	private float maxDistance = 0; // start at 0
	//private float minScale = 10000000000;
	//private float maxScale = 0;

    private int lastMapObjectsSize;
	// Use this for initialization
	void Start () {
        CheckGuiPopups(); // check to see if we started out with some map objects, if so, populate our guiPopupObjects
        lastMapObjectsSize = mapObjects.Count;
	}
	
	// Update is called once per frame
	void Update () {

        CheckOnScreen();

        if (lastMapObjectsSize != mapObjects.Count)
        {
            CheckGuiPopups();
        }


		if (displayMap) {
			CalculateGuiPopupValues(); // calcualte distance and scale from the ship to each map object for each guiPopupItem
			DisplayMap();
			//TODO: if not active, set active
			//TODO: if uncheck, set back to un-active
		}

        lastMapObjectsSize = mapObjects.Count;
	}

    //loop through all the map objects and add their guiPopupObjects to our list.
    void CheckGuiPopups()
    {
        foreach (GameObject mapObject in mapObjects)
        {
            if (mapObject.GetComponent<MapObject>() != null)
            {
                /*
                 * // used if we need to instantiate a prefab..... for now I think we will have the generators create a GameObject for their guiPopUp, instead of using a prefab
               GameObject guiPopup = Instantiate(mapObject.GetComponent<MapObject>().guiPopupPrefab, this.transform.position, mapObject.transform.rotation) as GameObject;
                GuiPopup popupScript = guiPopup.GetComponent<GuiPopup>();
                popupScript.mapObject = mapObject;
                popupScript.scale = mapObject.transform.lossyScale.magnitude;
                guiPopup.name = mapObject.gameObject.name + "GuiPopUp";
                guiPopups.Add(guiPopup);
                guiPopup.GetComponent<MeshRenderer>().enabled = false;
                 * */
                GameObject guiPopup = mapObject.GetComponent<MapObject>().guiPopup;
                if(!guiPopups.Contains(guiPopup))
                {
                    guiPopups.Add(guiPopup);
                }
            }
            else
            {
                Debug.LogError("MapObject does not have a GuiPopup script attached", mapObject);
            }
        }

    }

	// Get the position and scale of each guiPopupItem. also display info like their name, distance from the ship, etc.
	void CalculateGuiPopupValues()
	{
		foreach (GameObject guiPopup in guiPopups)
		{
			GuiPopup popupScript = guiPopup.GetComponent<GuiPopup>(); // get the map object related to this guiPopupItem
			GameObject mapObject = popupScript.mapObject;
			popupScript.distance = Vector3.Distance(this.transform.position,mapObject.transform.position); // get the distance to that object
			if(popupScript.distance > maxDistance)
			{
				maxDistance = popupScript.distance;
			}
			if(popupScript.distance < minDistance)
			{
				minDistance = popupScript.distance;
			}
		}
	}
	// display the guiPopup items around the ship.
	void DisplayMap()
	{
		float mapRadius = mapEndRadius - mapStartRadius;
		float totalMapObjectDistance = maxDistance - minDistance; 

		//minDistance should be displayed at MapStartRadius
		//maxDistance should be displayed at MapEndRadius
		foreach (GameObject guiPopup in guiPopups)
		{
			if(guiPopup.GetComponent<MeshRenderer>().enabled == false)
			{
				guiPopup.GetComponent<MeshRenderer>().enabled = true;
			}
			GuiPopup popupScript = guiPopup.GetComponent<GuiPopup>();
			Vector3 toPopup = (popupScript.mapObject.transform.position - this.transform.position).normalized; // normalized vector pointing towards the mapObject
			//distance we should display the guiPopupAt

            if (totalMapObjectDistance > 0)
            {
                float guiPopupDistance = mapStartRadius + ((popupScript.distance - minDistance) / totalMapObjectDistance) * (mapRadius); // distance = mapStartRadius + (% objectDistance compared to maxObjectDistance)*(mapSize)
                guiPopup.transform.position = this.transform.position + toPopup * guiPopupDistance;//set the guiPopup position
            }
            else// error when we only have 1 map item, totalMapObjectDistance == 0. if that's the case, just use 1/2 mapRadius
            {
                guiPopup.transform.position = this.transform.position + toPopup * mapRadius * .5f ;
            }
               
		}
		
	}

	void CheckOnScreen()
	{
		List<GameObject> toRemove = new List<GameObject> ();

		foreach (GameObject mapObject in mapObjects)
		{
			Vector3 screenPoint = Camera.main.WorldToScreenPoint(mapObject.transform.position);

			//if on the screen
			if(screenPoint.x > 0 && screenPoint.x < Screen.width && screenPoint.y > 0 && screenPoint.y < Screen.height)
			{
				if(!onScreen.Contains(mapObject))
				{
					onScreen.Add(mapObject);
				}
			}
			// if off the screen
			else{
				if(onScreen.Contains(mapObject))
				{
					toRemove.Add(mapObject);
				}
			}
		}

		foreach( GameObject r in toRemove)
		{
			onScreen.Remove(r);
		}

	}


}


