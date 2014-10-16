using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
 * abstract tool to be inherited by anything that is damagable.
 * 
 * */ 
public class Health : MonoBehaviour {

	public List<GameObject> scrapComponents;
    public GameObject HealthBar;
    public int numHealthDivisions;
	public float scrapScale;
	public int health;
	public int maxHealth;
	public int scrapVelocity;
    public bool showHealth;
	public bool isDead = false;

    private float initialHealthBarWidth;
    private int lastHealth;
	// Use this for initialization
	void Start () {
		//health = maxHealth;
        initialHealthBarWidth = 0;
        if (HealthBar != null)
        {
            initialHealthBarWidth = HealthBar.transform.localScale.x;

            // initially scale the healthbar, for testing we might not always start with full health
            float newWidth = initialHealthBarWidth * ((float)health / maxHealth);
            HealthBar.transform.localScale = new Vector3(newWidth, HealthBar.transform.localScale.y, HealthBar.transform.localScale.z);
        }
	}
	
	// Update is called once per frame
	void Update () {

        //if we've been damaged
        if(health != lastHealth)
        {
            if (HealthBar != null)
            {
                float newWidth = initialHealthBarWidth * ((float)health / maxHealth);
                //Debug.Log("Updating Healthbar: " + newWidth);
                HealthBar.transform.localScale = new Vector3(newWidth, HealthBar.transform.localScale.y, HealthBar.transform.localScale.z);
            }
        }
        lastHealth = health;
	}

	// Destroys this game object and spawns a random set of scrapComponents at a given scale.
	// 
	public void Split()
	{
		//spawn up to scrapComponents.Count() components, give them an outward motion, and a little torque.
		if(scrapComponents.Count > 0)
		{

			List<int> indexes = new List<int>(); // record the list of the index's we've already used
			for(int i = 0; i < scrapComponents.Count; i++)
			{
				indexes.Add(i);
			}
			int spawnCount = 0;
			if(scrapComponents.Count > 0)
			{
				spawnCount = Random.Range(1,scrapComponents.Count); // get a random # of items to spawn
			}
			else{
				spawnCount = Random.Range(0,scrapComponents.Count); // get a random # of items to spawn
			}
			Debug.Log("Creating " + spawnCount + "objects");
			for(int i = 0; i < spawnCount; i++) // for the # of items we want to spawn
			{
				//get a random index from the list
				int rand = Random.Range(0,indexes.Count);

                //get a random offset position
                Vector3 offsetPosition = Random.insideUnitSphere;

				//use it to spawn a scrap
				GameObject scrap = GameObject.Instantiate(scrapComponents[rand],this.transform.position + offsetPosition,this.transform.rotation) as GameObject;
				scrap.transform.localScale = this.transform.lossyScale*scrapScale;
				scrap.rigidbody.AddTorque(this.rigidbody.angularVelocity);
				Vector3 randomDirection = new Vector3(Random.value, Random.value, Random.value);
				scrap.rigidbody.AddForce(this.rigidbody.velocity*scrapVelocity+randomDirection*scrapVelocity); //velocity + velocityDirection*scrapVelocity
				//remove the index from the list
				indexes.Remove(rand);
			}
		}
	}


	public void Damage(int damage)
	{
		this.health -= damage;
		if(health <= 0)
		{
			Split ();
            //Debug.Log(this.gameObject.name + " is Dead");
           
            // we only want to destroy this if it doesn't have a list of destructable objects
            //otherwise it destroys this item first, and then none of the destructable objects get destroyed
			if(this.GetComponent<DestructableObject>() == null) 
            {
                Destroy(this.gameObject);
            }
			//TODO: if shipController.target == this.gameObject, shipController.target = null
			//TODO: or even better, autoset the new target = to one of the scrapComponents
		}
	}
} 