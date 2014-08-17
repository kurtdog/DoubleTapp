using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {


	public string name;
	public int damage;
	public float speed;
	public Vector2 force;

	public float lifeTime;
	private float timer;
	GameObject parentGameObject;
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
		//Debug.Log("hitting " + col.gameObject.name);
		if(col.gameObject != parentGameObject) // only go on if we didn't hit our parentObject
		{
			//Debug.Log("not parent");
			if(col.gameObject.GetComponent<Health>() != null ) // we don't want to hurt ourself
			{
				//Debug.Log("hit Game Item");
				col.gameObject.GetComponent<Health>().Damage(damage); //apply damage

			}
			Destroy(this.gameObject);
		}
	}

	public void setParentGameObject( GameObject go)
	{
		this.parentGameObject = go;
	}

}
