using UnityEngine;
using System.Collections;

public class ForceField : MonoBehaviour {

    public GameObject ParentShip;
    public float lerpSpeed;
    public float effectTime;
    public float alphaMin;
    public float alphaMax;
    public float alpha;
    private bool playEffect = false;
    private float solidTimer;
    private float transparentTimer;

	// Use this for initialization
	void Start () {
        solidTimer = effectTime;
        transparentTimer = effectTime;

	}
	
	// Update is called once per frame
	void Update () {


        solidTimer += Time.fixedDeltaTime;
        transparentTimer += Time.fixedDeltaTime;

        if (solidTimer < effectTime)
        {
            //Debug.Log("Solidifying");
            MakeSolid();
        }
        else
        {
            transparentTimer = 0;
        }

        if (transparentTimer < effectTime)
        {
            //Debug.Log("MakingTransparent");
            MakeTransparent();
        }

        /*
        Color color = renderer.material.color;
        color.a = alpha;
        this.gameObject.renderer.material.color = color;
         * */
    }

    void OnTriggerEnter(Collider col)
    {
       
        //TODO: , less ahcky, after spawning projectile, make it's parent the thing that shot it. Then compare it's parent to public Ship
        if (col.gameObject.name != ParentShip.GetComponent<Shooter>().projectile.name + "(Clone)") 
        {
            Debug.Log("(" + col.gameObject.name + "," + ParentShip.GetComponent<Shooter>().projectile.name + "(Clone)" + ")");
            //Debug.Log("Trigger");
            playEffect = true;
            solidTimer = 0;
        }
    }


    void MakeSolid()
    {

        alpha = Mathf.Lerp(alphaMax, alphaMin, Time.fixedDeltaTime * lerpSpeed);
        //alpha += lerpSpeed;
        //Debug.Log("playingEffect: " + alpha);
        Color color = renderer.material.color;
        color.a = alpha;
        this.gameObject.renderer.material.color = color;
        Camera.main.Render();
    }


    void MakeTransparent()
    {

        alpha = Mathf.Lerp(alphaMin, alphaMax, Time.fixedDeltaTime * lerpSpeed);
        //alpha = alphaMax;
        //Debug.Log("playingEffect: " + alpha);
        Color color = renderer.material.color;
        color.a = alpha;
        this.gameObject.renderer.material.color = color;
        Camera.main.Render();
    }
}
