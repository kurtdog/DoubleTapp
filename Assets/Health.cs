using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
 * abstract tool to be inherited by anything that is damagable.
 * 
 * */ 
public class Health : MonoBehaviour {

	public List<GameObject> scrapComponents;
	public float scrapScale;
	public int health;
	public int maxHealth;
	public int scrapVelocity; 
	public bool isDead = false;

	// Use this for initialization
	void Start () {
		health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
	
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

				//use it to spawn a scrap
				GameObject scrap = GameObject.Instantiate(scrapComponents[rand],this.transform.position,this.transform.rotation) as GameObject;
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
		if(health < 0)
		{
			Split ();
			Destroy(this.gameObject);
			//TODO: if shipController.target == this.gameObject, shipController.target = null
			//TODO: or even better, autoset the new target = to one of the scrapComponents
		}
	}
} 