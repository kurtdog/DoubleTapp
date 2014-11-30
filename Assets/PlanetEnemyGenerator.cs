using UnityEngine;
using System.Collections;

/*NOTE: this is added to each generated planet.
 * I don't have the script attached to the emptyPlanetPrefab because then map objects would have them too.
 * 
 * */
public class PlanetEnemyGenerator : Generator {

    public EnemySet enemySet;
    public GameObject planet;
    public GameObject Ship;
    public int generationDistance; // distance to generate the objects in the enemyset.
    //min and max height off of the planet to generate our ships
    public int minGenHeight;
    public int maxGenHeight;
    Planetary.Planet planetScript;

	// Use this for initialization
	void Start () {
        ParentStart();

        //get the number of fighters, carriers, and bosses to spawn based on the min and max settings in enemyGenSettings
        enemySet.fightersToSpawn = Random.Range(enemySet.minFighters, enemySet.maxFighters);
        enemySet.carriersToSpawn = Random.Range(enemySet.minCarriers, enemySet.maxCarriers);
        enemySet.bossesToSpawn = Random.Range(enemySet.minBosses, enemySet.maxBosses);

        

        itemsToSpawn = enemySet.fightersToSpawn + enemySet.carriersToSpawn + enemySet.bossesToSpawn;
	}
	
	// Update is called once per frame
    void Update()
    {
        ParentUpdate();
    }

    public override void Pop()
    {
        SpawnEnemyFromEnemySet();
    }

    public void SpawnEnemyFromEnemySet()
    {
        //get the % of each category in th enemyset. i.e fighetsToSpawn/itemsToSpawn,  carriersToSpawn/itemsToSpawn
        float fighterChance = (float)enemySet.fightersToSpawn / itemsToSpawn;
        float carrierChance = (float)enemySet.carriersToSpawn / itemsToSpawn;
        float bossChance = (float)enemySet.bossesToSpawn / itemsToSpawn;

        //depending on the chance values, spawn an enemy from the enemy set.
        float random = Random.value;
        if(random < fighterChance)
        {
            //spawn fighter
            GameObject fighter = Instantiate(enemySet.GetFighter(),GetSpawnPosition(),this.transform.rotation) as GameObject;
            fighter.GetComponent<AIController>().PlayerShip = Ship;
            enemySet.fightersToSpawn--;
            itemsToSpawn--;
            spawnedObjects.Add(fighter);
        }
        else if(random < (fighterChance + carrierChance))
        {
            //spawn carrier
            GameObject carrier = Instantiate(enemySet.GetCarrier(), GetSpawnPosition(), this.transform.rotation) as GameObject;
            carrier.GetComponent<AIController>().PlayerShip = Ship;
            enemySet.carriersToSpawn--;
            itemsToSpawn--;
            spawnedObjects.Add(carrier);
        }
        else
        {
            //spawn boss
            GameObject boss = Instantiate(enemySet.GetBoss(), GetSpawnPosition(), this.transform.rotation) as GameObject;
            boss.GetComponent<AIController>().PlayerShip = Ship;
            enemySet.bossesToSpawn--;
            itemsToSpawn--;
            spawnedObjects.Add(boss);

        }

    }


    //get a random position on the planet to spawn the enemy
    Vector3 GetSpawnPosition()
    {
        
        if (planetScript == null)
        {
            planetScript = planet.GetComponent<Planetary.Planet>();

        }


        //spawn a fighter at the right radius, at a random point on the planet
        //Vector3 planetToSpawnPoint = (Ship.transform.position + Ship.transform.forward.normalized * generationDistance) - planet.transform.position; //(shipPosition + offset) - planetPosition
        

        float randomHeight = planetScript.radius + Random.Range(minGenHeight, maxGenHeight); // get a distance away from the planet to spawn the object
        Vector3 spawnPostion = planet.transform.position + Random.onUnitSphere* randomHeight;//Ship.transform.position + Random.onUnitSphere * 2;//

        return spawnPostion;
    }
}
