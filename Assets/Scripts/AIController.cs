using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIController : MonoBehaviour {

	public GameObject PlayerShip;
	public Transform ShooterShipTransform;
    public List<Behavior> behaviors;

    //Attacking
    [HideInInspector] public int attackDistance;
    [HideInInspector] public int fireDistance;
    [HideInInspector] public float fireRate;
    [HideInInspector] public GameObject Projectile;
    [HideInInspector] public GameObject ShotPointNetwork;
	//Movement
    [HideInInspector] public float acceleration;
    [HideInInspector] public float distance;
    //Rotation
    [HideInInspector] public float rotationSpeed; // for AiState(Rotate)
    public Vector3 rotationAxis; //TODO: figure out vector3 custom inspector
    //Spawning
    [HideInInspector] public GameObject SpawnPointNetwork;
    [HideInInspector] public float spawnRate;
    public List<GameObject> EnemiesToSpawn;
    [HideInInspector] public int maxEnemySpawns;
    //Unloading, i.e., AI that unload, not the carrier ai's
    [HideInInspector]
    public float unloadTime; //Unload, UNLOAD's only purpose is to show 'unloadTime'

    public enum Behavior { FREEROAM, SHOOTATPLAYER, FLYSTRAIGHT ,LOOKATPLAYER, ROTATE, SPAWNENEMIES, UNLOAD }; // General Actions that AI's can perform

    PointNetwork shotPointNetwork;
    PointNetwork spawnPointNetwork;

	private List<GameObject> projectiles;
    private List<GameObject> enemies;
    private float shotTimer;
    private float spawnTimer;
    private float unloadTimer;
    private int shotCount;
    private int spawnCount;
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
        shotPointNetwork = ShotPointNetwork.GetComponent<PointNetwork>();
        spawnPointNetwork = SpawnPointNetwork.GetComponent<PointNetwork>();
		projectiles = new List<GameObject>();
        enemies = new List<GameObject>();
        shotTimer = 0;
        spawnTimer = 0;
        unloadTimer = 0;
        shotCount = 0;
        spawnCount = 0;

	}
	
	// Update is called once per frame
	void Update () {
	
		shotTimer += Time.fixedDeltaTime;
        spawnTimer += Time.fixedDeltaTime;

        unloadTimer += Time.fixedDeltaTime;
            
        

		distance = Vector3.Distance(this.transform.position,ShooterShipTransform.position);

        HandleBehaviors();

	}

	void HandleBehaviors()
	{
		//Debug.Log("Attacking");

        if(unloadTimer > unloadTime)
        {
            if (behaviors.Contains(Behavior.LOOKATPLAYER))
            {
                LookAt();
            }

            if (behaviors.Contains(Behavior.FLYSTRAIGHT))
            {
                FlyStraight();
            }

            if (behaviors.Contains(Behavior.SHOOTATPLAYER))
            {
                ShootAtPlayer();
            }

        
            if (behaviors.Contains(Behavior.ROTATE) ) // don't rotate if unloading, only move forward
            {
               
                Rotate();
            }

            if (behaviors.Contains(Behavior.SPAWNENEMIES) && spawnCount < maxEnemySpawns)
            {
                SpawnEnemies();
            }
        }
        else
        {
            FlyStraight();
            //Debug.Log("unloading");
        }
	}

    void Rotate()
    {

        this.transform.Rotate(rotationAxis, rotationSpeed * Time.fixedDeltaTime);
    }

    void SpawnEnemies()
    {
        if (spawnCount < maxEnemySpawns)
        {
            if (spawnTimer > spawnRate)
            {
        
                
                GameObject enemyPrefab = EnemiesToSpawn[Random.Range(0, EnemiesToSpawn.Count - 1)]; // get random enemy
                GameObject spawnPoint = spawnPointNetwork.points[Random.Range(0, spawnPointNetwork.points.Count - 1)]; //get random spawn point
                GameObject enemy = Instantiate(enemyPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
                //Debug.Log("Spawning at : " + spawnPoint.name);
                enemy.GetComponent<AIController>().setShip(this.PlayerShip);
                enemy.GetComponent<AIController>().setShooter(this.ShooterShipTransform);
                


                Vector3 f = enemy.transform.forward * (enemy.GetComponent<AIController>().acceleration);
                //bullet.GetComponent<Projectile>().force = f;
                //Debug.Log("adding force f: " + f);
                enemy.rigidbody.AddForce(f);
                //bullet.rigidbody.velocity = f;
                enemies.Add(enemy);
                spawnCount++;
                spawnTimer = 0;
            }
        }
        
    }

    int getRandomIndex(List<GameObject> list)
    {
        int index = Random.Range(0, list.Count - 1);

        return index;

    }
	void ShootAtPlayer()
	{
        
		if(shotTimer > 1/fireRate)
		{
            if(shotCount < shotPointNetwork.points.Count)
            {
            //Debug.Log("Shooting");
            GameObject shotPoint = shotPointNetwork.points[shotCount];
            if (shotPoint.activeSelf)
            {
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
                shotCount++;
            }
            }
            else
            {
                //instead of looping through all shotpoints, only do 1 per update, that way if an enemy shoots a ton of bullets,
                //it doesn't lag the game from all the instantiations.
                shotTimer = 0;
                shotCount = 0;
            }
		}
	}


	//flight methods
    void LookAt()
	{
		//Debug.Log("Flying");
        Vector3 targetDirection = PlayerShip.transform.position - this.transform.position;
        this.transform.LookAt(PlayerShip.transform.position); // LookAt the target

	}
    void FlyStraight()
    {
        //Debug.Log("Flying");
        this.rigidbody.AddForce(this.transform.forward * acceleration); // move towards it

    }


    void setShip(GameObject ship)
    {
        this.PlayerShip = ship;
    }

    void setShooter(Transform shooter)
    {
        this.ShooterShipTransform = shooter;
    }
}
