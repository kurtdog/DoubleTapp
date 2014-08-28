using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ParticleGenerator : Generator {

    public List<GameObject> particleSystems;

    void Start()
    {
        spawnedObjects = new List<GameObject>();
        itemsToSpawn = Random.Range(1, 6);
    }

    //spawn 1 random particle system
	public override void Pop()
    {
        //get a random number of particle systems, 0-4
        //generate them all.
        //maybe have an enum ParticleType{CLOUD,BELT,SPIRAL,ETC}.. in a particle controller attached to each particle system.
        //TODO: if cloud then spawn a couple?

        GameObject particleSys = Instantiate(particleSystems[Random.Range(0,particleSystems.Count)], this.transform.position, this.transform.rotation) as GameObject;
        ParticleScript particleScript = particleSys.GetComponent<ParticleScript>();
        ParticleSystem particleSystem = particleSys.GetComponent<ParticleSystem>();
        if (particleScript != null)
        { 
           // particleSystem.startColor = particleScript.getAvailableColor();
            
            if (particleScript.particleType == ParticleScript.ParticleType.CLOUD)
            {
                particleSys.rigidbody.AddTorque(GetRandomTorque());
                particleSys.transform.position = getRandomPosition();
            }
            

        }


        particleSys.transform.parent = this.transform;
        spawnedObjects.Add(particleSys);
        

    }

    Vector3 getRandomPosition()
    {

        return (this.transform.forward*Random.Range(0,100));
    }

    


}
