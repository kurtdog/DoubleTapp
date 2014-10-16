using UnityEngine;
using System.Collections;

/*
 * Debug distances by drawing a line
 * */
public class DistanceDebugLine : MonoBehaviour {

    public GameObject Target;
    public float distance;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 toTarget = Target.transform.position - this.transform.position;

        Debug.DrawLine(this.transform.position, toTarget.normalized * distance);
	
	}
}
