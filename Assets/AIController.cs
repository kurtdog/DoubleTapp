using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIController : MonoBehaviour {

	public GameObject PlayerShip;
	public Transform ShooterShipTransform;
	public GameObject Projectile;
	public GameObject ShotPointNetwork;
	public float distance;
	public float fireRate;
	public int attackDistance;
	public int fireDistance;
	//public float maxSpeed;
	public float acceleration;
    public float rotationSpeed; // for AiState(Rotate)

    public List<Behavior> behaviors;
    public enum Behavior { FREEROAM, SHOOTATPLAYER, CHASE, ROTATE }; // General Actions that AI's can perform
    ShotPointNetwork shotPointNetwork;

	private List<GameObject> projectiles;
	private float shotTimer = 0;
	//public enum FreeRoamMethod (STAYPUT,PATROL,GORANDOMDIR)
	/*
	 *  Note, we should have a dropDown of attack methods, flight patters, etc
	 * That way we can have one robjust AI controller, and then for each AI type, we can just assign the same controller, and pick and choose settings.
	 * like AttackMethod - followDirect, followIndirect, slowChase
	 * FreeRoam - patrol, randomDirects, etc.
	 * 
	 * Following this paradigm will also allow people to create their own AI.
	 */ 
	// Use this for initialization
	void Start () {
        shotPointNetwork = ShotPointNetwork.GetComponent<ShotPointNetwork>();
		projectiles = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
	
		shotTimer += Time.fixedDeltaTime;
		distance = Vector3.Distance(this.transform.position,ShooterShipTransform.position);

        HandleBehaviors();

	}

	void HandleBehaviors()
	{
		//Debug.Log("Attacking");


        if (behaviors.Contains(Behavior.CHASE))
        {
            FlyStraightTowardsPlayer();
        }

        if (behaviors.Contains(Behavior.SHOOTATPLAYER))
        {
            ShootAtPlayer();
        }

        if (behaviors.Contains(Behavior.ROTATE))
        {
            Rotate();
        }
		
	}

    void Rotate()
    {

        this.transform.Rotate(this.transform.up, rotationSpeed*Time.fixedDeltaTime);
    }

	void ShootAtPlayer()
	{

		if(shotTimer > 1/fireRate)
		{
            foreach (GameObject shotPoint in shotPointNetwork.shotPoints) 
            {
                Debug.Log("Shooting");
                GameObject bullet = Instantiate(Projectile, shotPoint.transform.position, shotPoint.transform.rotation) as GameObject;
                bullet.GetComponent<Projectile>().setParentGameObject(this.gameObject);

                Vector3 shotDirection = ShooterShipTransform.transform.position - shotPoint.transform.position;
                bullet.transform.forward = shotDirection;

                Vector3 f = shotDirection * (bullet.GetComponent<Projectile>().speed + Mathf.Abs(this.rigidbody.velocity.magnitude));
                //bullet.GetComponent<Projectile>().force = f;
                //Debug.Log("adding force f: " + f);
                bullet.rigidbody.AddForce(f);
                //bullet.rigidbody.velocity = f;
                projectiles.Add(bullet);
                shotTimer = 0;
            }
		}
	}


	//flight methods
	void FlyStraightTowardsPlayer()
	{
		//Debug.Log("Flying");
        Vector3 targetDirection = PlayerShip.transform.position - this.transform.position;
        this.transform.LookAt(PlayerShip.transform.position); // LookAt the target
		this.rigidbody.AddForce(this.transform.forward*acceleration); // move towards it

	}
}
