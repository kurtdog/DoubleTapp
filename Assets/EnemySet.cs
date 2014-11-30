using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySet : MonoBehaviour {

    public List<GameObject> fighterShips;
    public List<GameObject> carrierShips;
    public List<GameObject> bossShips;


    //min and max values to be set, how many fighters, carriers, and bosses to spawn
    public int minFighters;
    public int maxFighters;
    public int minCarriers;
    public int maxCarriers;
    public int minBosses;
    public int maxBosses;

    //values that get set based on the min and max settings
    public int fightersToSpawn;
    public int carriersToSpawn;
    public int bossesToSpawn;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public GameObject GetFighter()
    {
        int index = Random.Range(0, fighterShips.Count);
        return fighterShips[0];
    }

    public GameObject GetCarrier()
    {
        int index = Random.Range(0, carrierShips.Count);
        return carrierShips[0];

    }

    public GameObject GetBoss()
    {
        int index = Random.Range(0, bossShips.Count);
        return bossShips[0];
    }
}
