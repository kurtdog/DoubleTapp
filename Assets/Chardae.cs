using UnityEngine;
using System.Collections;

public class Chardae : MonoBehaviour {

    int integer;
    float floatValue = 1.4f;
    bool isSexy = true;

	// Use this for initialization
	void Start () {
        if(isSexy)
        {
            Debug.Log("Chardae is gorgy");
        }
        int i = 0;
        while(i < 10)
        {
            Debug.Log("i: " + i);
            i++;
        }

        for (int j = 5; j < 15; j++ )
        {
            Debug.Log("j: " + j);
        }

	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("Chardae is daelicous!");
	}
}
