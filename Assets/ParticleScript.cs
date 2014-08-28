using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleScript : MonoBehaviour {
   
    public List<Color> availableColors;
    public int minSize;
    public int maxSize;
    public int minParticleSize;
    public int maxParticleSize;
    public ParticleType particleType;
    public enum ParticleType { BELT, SPIRAL, CLOUD };
    public float rotationSpeed; // used for spiral, cloud

	// Use this for initialization
	void Start () {
        this.GetComponent<ParticleSystem>().startColor = getAvailableColor();
        float size = Random.Range(minSize, maxSize);
        this.transform.localScale += new Vector3(size, size, size);
        particleSystem.startSize = particleSystem.startSize * size;
	}
	
	// Update is called once per frame
	void Update () {
        if (particleType == ParticleType.SPIRAL)
        {
            this.transform.Rotate(this.transform.forward, rotationSpeed);
        }


	}

    public Color getAvailableColor()
    {
        if (availableColors.Count > 0)
        {
            return availableColors[Random.Range(0, availableColors.Count)];
        }
        else
        {
            return Color.white;
            Debug.Log("ERROR, availble colors for particleScript " + this + " haven't been set");
        }
    }
}
