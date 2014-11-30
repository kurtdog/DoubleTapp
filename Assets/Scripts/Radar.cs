using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Radar : MonoBehaviour {

    //use a collider for detectionDistance

    public GameObject radarBlipPrefab;
    public List<GameObject> targets; //TODO: private
    public List<GameObject> radarBlips; //TODO: private
    public float radius ;

	// Use this for initialization
	void Start () {
	    targets = new List<GameObject>();
        radarBlips = new List<GameObject>();
        //radius = this.GetComponent<SphereCollider>().radius;
	}
	
	// Update is called once per frame
	void Update () {

        DestroyBlips();
	
	}

    void DestroyBlips()
    {
        GameObject toDestroy = null;
        foreach(GameObject blip in radarBlips)
        {
            if(blip.GetComponent<RadarBlip>().target == null)
            {
                toDestroy = blip;
                break;
            }
        }

        if(toDestroy != null)
        {
            radarBlips.Remove(toDestroy);
            Destroy(toDestroy);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (!targets.Contains(col.gameObject) && col.gameObject.GetComponent<AIController>() != null)
        {
            targets.Add(col.gameObject);
            Vector3 startPos = this.transform.position + this.transform.forward.normalized*radius; //start the blip right in fromnt of you
            GameObject blip = Instantiate(radarBlipPrefab,startPos,this.transform.rotation) as GameObject; // make a new blip at the above position
            RadarBlip rblip = blip.GetComponent<RadarBlip>();
            rblip.target = col.gameObject; // set the blips target
            rblip.transform.parent = this.transform; // set the blips parent to this
            rblip.radius = radius; // set the blips radius
            
            
        }
    }

    
    void OnTriggerExit(Collider col)
    {
        if (targets.Contains(col.gameObject)) ;
        {
            targets.Remove(col.gameObject);
            Debug.Log("removing target");
            DestroyBlipWithTarget(col.gameObject);
        }
    }

    void DestroyBlipWithTarget(GameObject t)
    {
        GameObject toDestroy = null;
        Debug.Log("Trying to destroy: " + t.name);
        foreach (GameObject blip in radarBlips)
        {
            if (blip.GetComponent<RadarBlip>().target.Equals(t))
            {
                toDestroy = blip;
                Debug.Log("toDestroy = blip");
                //break;
            }
        }

        if (toDestroy != null)
        {
            Debug.Log("removing blip");
            radarBlips.Remove(toDestroy);
            Destroy(toDestroy);
        }
    }
}
