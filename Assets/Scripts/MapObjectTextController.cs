using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapObjectTextController : MonoBehaviour {

	public GameObject Ship; // the players ship
	public GameObject MapObject; // the object we're displaying on the map, a planet, asteroid field, etc.

    private Map map;
	FloatingText floatingText;
	public List<string> strings;
	// Use this for initialization
	void Start () {
		floatingText = this.GetComponent<FloatingText>();
		strings = new List<string> ();
		strings.Add ("");
		strings.Add ("");
        map = Ship.GetComponentInChildren<Map>();
	}
	
	// Update is called once per frame
	void Update () {
		strings [0] = MapObject.name;
		strings [1] = "distance: " + Vector3.Distance (MapObject.transform.position, Ship.transform.position);

        if (map.displayMap && strings != null)
        {
            floatingText.DisplayText(strings);
        }
	}
}
