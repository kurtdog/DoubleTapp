using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//generic shooter class that uses the ShotPointNetwork item and spawns bullets.
public class Shooter : MonoBehaviour {

	public GameObject ShotPointNetwork;
	public Transform target;
	public GameObject projectile;
	public int shotCount;
	public float fireRate;
	private float shotTimer = 0;
	public ShotType shotType;
	public enum ShotType{StraightShot,Homing};
	private List<GameObject> projectiles;

	ShotPointNetwork shotPointNetwork;
	// Use this for initialization
	void Start () {
		shotPointNetwork = ShotPointNetwork.GetComponent<ShotPointNetwork>();
		projectiles = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		shotTimer += Time.fixedDeltaTime;
	}

	public void Shoot()
	{
		//Debug.Log("shooting");
		if(shotTimer > 1/fireRate)
		{
			shotTimer = 0;
			for(int i = 0; i < shotCount; i++)
			{
				GameObject bullet = Instantiate(projectile, shotPointNetwork.shotPoints[i].transform.position,this.transform.rotation) as GameObject;
				bullet.GetComponent<Projectile>().setParentGameObject(this.gameObject);
				AddShotBehavior(bullet,shotPointNetwork.shotPoints[i].transform);
				projectiles.Add(bullet);
			}
		}
	}

	void AddShotBehavior(GameObject bullet, Transform shotPosition)
	{
		switch(shotType)
		{
		case ShotType.StraightShot:
			Vector3 f = shotPosition.forward*(bullet.GetComponent<Projectile>().speed +Mathf.Abs(this.rigidbody.velocity.magnitude));
			bullet.rigidbody.AddForce(f);
			break;
		default: 
			break;
		}

	}

}
