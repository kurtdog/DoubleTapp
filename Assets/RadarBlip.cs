using UnityEngine;
using System.Collections;

public class RadarBlip : MonoBehaviour {

    public GameObject target;
    public float radius;

    private Radar radar;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (this.transform.parent != null && target != null)
        {
            Transform parent = this.transform.parent.transform;
            Vector3 toTarget = target.transform.position - parent.position;

            this.transform.position = parent.position + toTarget.normalized * radius;
            this.transform.LookAt(target.transform.position);
        }

	}


}
