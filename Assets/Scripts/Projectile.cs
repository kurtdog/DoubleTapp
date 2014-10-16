using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {


	public string name;
	public int damage;
	public float speed;
	public Vector2 force;
    public bool rayCast;

	public float lifeTime;
    public int rayCastLenght;
	private float timer;
    private float hitDistance = 1000;
    private Vector3 startPosition;
	GameObject parentGameObject;
    public LayerMask destructableLayer;
	// Use this for initialization
	void Start () {
		timer = 0;
       // Vector3 inFrontOfProjectile = this.transform.position + this.transform.forward * 2;
        if (rayCast)
        {
            Ray ray = new Ray(this.transform.position, this.transform.forward);
            RaycastHit hit;
//            Debug.Log("CastingRay");
            //for some reason my lasers are hitting each other!
            //either cast 1 ray for multiple lasers... or figure something else out
            if (Physics.Raycast(ray, out hit, rayCastLenght, destructableLayer))
            {
               // Debug.Log("RayCastHit!");
                if (hit.collider.gameObject.GetComponent<Health>() != null) // we don't want to hurt ourself
                {
                   // Debug.Log("Damaging " + hit.collider.gameObject.name);
                    hit.collider.gameObject.GetComponent<Health>().Damage(damage); //apply damage
                }
                else
                {
                    //Debug.Log("No Health Script attached to the collider object: " + hit.collider.gameObject.name);
                }
                hitDistance = Vector3.Distance(this.transform.position,hit.collider.gameObject.transform.position);
                startPosition = this.transform.position;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.fixedDeltaTime;

		if(timer > lifeTime)
		{
			Destroy(this.gameObject);
		}

        if (rayCast && Vector3.Distance(this.transform.position, startPosition) > hitDistance)
        {
            Destroy(this.gameObject);
        }
	}

    
	void OnCollisionEnter(Collision col)
	{
        if (!rayCast)
        {
            //Debug.Log("hitting " + col.gameObject.name);
            if (col.gameObject != parentGameObject) // only go on if we didn't hit our parentObject
            {
                //Debug.Log("not parent");
                if (col.gameObject.GetComponent<Health>() != null) // we don't want to hurt ourself
                {
                    Debug.Log("hit Game Item");
                    col.gameObject.GetComponent<Health>().Damage(damage); //apply damage
                }
                Destroy(this.gameObject);
            }
        }
	}
    

	public void setParentGameObject( GameObject go)
	{
		this.parentGameObject = go;
	}

}
