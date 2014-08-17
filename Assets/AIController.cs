using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIController : MonoBehaviour {

	public GameObject PlayerShip;
	public Transform ShooterShipTransform;
	public GameObject Projectile;
	public GameObject ShotPosition;
	public float distance;
	public float fireRate;
	public int attackDistance;
	public int fireDistance;
	//public float maxSpeed;
	public float acceleration;

	public AiState aiState;
	public AttackMethod attackMethod;
	public enum AiState{FREEROAM,ATTACK,DEFEND}; // General Actions that AI's can perform
	public enum AttackMethod {ORBIT,SIMPLE,AGGRESIVE,RELAXED}; //fine grain actions, different attack algorithms

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
		projectiles = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
	
		shotTimer += Time.fixedDeltaTime;
		distance = Vector3.Distance(this.transform.position,ShooterShipTransform.position);
		switch(aiState)
		{
		case AiState.ATTACK:
			Attack ();
			break;

		default:
			break;
		}
	}

	void Attack()
	{
		//Debug.Log("Attacking");

		if( distance< attackDistance)
		{
			switch(attackMethod)
			{
			case AttackMethod.ORBIT:
				Orbit();
				break;
			default: 
				break;
			}
		}
	}

	/*
	 * Fly directly towards ship, and fire when within fireDistance
	 * */
	void Orbit()
	{
		//Debug.Log("AttackSimple");
		FlyStraightTowardsTarget(PlayerShip.gameObject);
		Shoot(PlayerShip.gameObject,ShotPosition);
	}

	void Shoot(GameObject target,GameObject ShotPosition)
	{

		if(shotTimer > 1/fireRate)
		{
			//Debug.Log("Shooting");
			GameObject bullet = Instantiate(Projectile, ShotPosition.transform.position,this.transform.rotation) as GameObject;
			bullet.GetComponent<Projectile>().setParentGameObject(this.gameObject);
			
			Vector3 shotDirection = target.transform.position - this.transform.position;
			
			Vector3 f = shotDirection*(bullet.GetComponent<Projectile>().speed +Mathf.Abs(this.rigidbody.velocity.magnitude));
			//bullet.GetComponent<Projectile>().force = f;
			//Debug.Log("adding force f: " + f);
			bullet.rigidbody.AddForce(f);
			//bullet.rigidbody.velocity = f;
			projectiles.Add(bullet);
			shotTimer = 0;
		}
	}


	//flight methods
	void FlyStraightTowardsTarget(GameObject target)
	{
		//Debug.Log("Flying");
		Vector3 targetDirection = target.transform.position - this.transform.position;
		this.transform.LookAt(target.transform.position); // LookAt the target
		this.rigidbody.AddForce(this.transform.forward*acceleration); // move towards it

	}
}
