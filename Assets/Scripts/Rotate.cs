using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

    public Vector3 axis;
    public int rotationSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        this.transform.Rotate(axis, rotationSpeed);
	}
}
