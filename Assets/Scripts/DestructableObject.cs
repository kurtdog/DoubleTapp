using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestructableObject : MonoBehaviour {

    public List<GameObject> childrenToDestroy;

    public bool objectsDestroyed = false;
    Health healthScript;
	// Use this for initialization
	void Start () {
        healthScript = this.GetComponent<Health>();
	}
	
	// Update is called once per frame
	void Update () {
        
        if (healthScript.health <= 0)
        {
            foreach(GameObject go in childrenToDestroy)
            {
                go.SetActive(false);
            }
            objectsDestroyed = true;
        }
	}
}
