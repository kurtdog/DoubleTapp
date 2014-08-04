using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {


	public string name;
	public int damage;
	public float speed;
	public Vector2 force;

	public float lifeTime;
	private float timer;

	// Use this for initialization
	void Start () {
		timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.fixedDeltaTime;

		if(timer > lifeTime)
		{
			Destroy(this.gameObject);
		}
	}

	void OnCollisionEnter(Collision col)
	{
		//if(col.gameObject.GetComponent<AIScript>())
		//{
			//apply damage
		//}
		Destroy(this.gameObject);
	}


}
