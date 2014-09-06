using UnityEngine;
using System.Collections;

public class MeshColor : MonoBehaviour {

    public Color color;

    public float alpha;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        color.a = alpha;
        this.gameObject.renderer.material.color = color;

	}


    void updateColor()
    {

    }
}
