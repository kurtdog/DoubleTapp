using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleScript : MonoBehaviour {
   
    private List<Color> availableColors;
    public int minParticleSize;
    public int maxParticleSize;
    public ParticleType particleType;
    public enum ParticleType { BELT, SPIRAL, CLOUD , HIGHWAY};
    public float rotationSpeed; // used for spiral, cloud

	// Use this for initialization
	void Start () {
        availableColors = new List<Color>();
        availableColors.Add(Color.magenta);
        availableColors.Add(Color.cyan);
        availableColors.Add(Color.red);

        this.GetComponent<ParticleSystem>().startColor = getAvailableColor();
        //this.GetComponent<ParticleSystem>().startColor = Color.magenta;
        //ParticleEmitter pe = this.GetComponent<ParticleEmitter>();
     
        Debug.Log(("startColor = " + this.GetComponent<ParticleSystem>().startColor));
        float size = Random.value;
        //this.transform.localScale += new Vector3(size, size, size);
        particleSystem.startSize = particleSystem.startSize * size;

        if(particleType == ParticleType.HIGHWAY)
        {
            this.transform.Rotate(this.transform.forward, Random.Range(0,180));
        }
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
            int rand = Random.Range(0, availableColors.Count-1);
            Color returnColor = availableColors[rand];
            Debug.Log("gettin color" + availableColors[rand]);
            return returnColor;
        }
        else
        {
            return Color.white;
            Debug.Log("ERROR, availble colors for particleScript " + this + " haven't been set");
        }
    }
}
