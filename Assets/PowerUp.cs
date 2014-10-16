using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

    public GameObject Ship;
    //public GameObject SHooterShip;

    public int healthToAdd;
    public float acceleration;
    public float maxSpeed;
    public float adjustmentSpeed;
    public float rotationSpeed;
    public float rotationIncrease; // speed up rotation by this amount
    public float pickupSpeed; // speed at which the object moves in towards the player after it has been 'captured'

    public int captureDistance; // distance until the item is 'captured' by the Ship.
    public bool captured = false;
    private Vector3 cross; 

	// Use this for initialization
	void Start () {
        cross = this.transform.up;
	}
	
	// Update is called once per frame
	void Update () {
	    float distance = Vector3.Distance(this.transform.position,Ship.transform.position);
        Vector3 direction = Ship.transform.position - this.transform.position;
        

        if (distance > captureDistance && !captured)
        {
            //move towards player
            if (this.rigidbody.velocity.magnitude < maxSpeed)
            {
                this.rigidbody.AddForce(direction * acceleration); //*direction.magnitude
            }
                //if we pass the player
            float dot = Vector3.Dot(this.rigidbody.velocity.normalized, direction.normalized);
            //Debug.Log(dot);
            if (dot < 0) // if we've moved past the player, slow the item down
            {
                this.rigidbody.AddForce(-this.rigidbody.velocity * adjustmentSpeed);
            }

            //calculate the cross while we're chasing. Then after we've been captured, use the cross as the rotation axis
            cross = Vector3.Cross(this.rigidbody.velocity, direction);
        }
        else
        {
            //set the Ship as a parent object
            if(this.transform.parent != Ship.transform)
            {
                this.transform.parent = Ship.transform;
                captured = true;
                this.rigidbody.velocity = new Vector3(0, 0, 0);
            }

            if (rotationSpeed < 20)
            {
                rotationSpeed += rotationIncrease;
               // Debug.Log("rotation: " + rotationSpeed);
            }

            this.transform.RotateAround(Ship.transform.position, cross, rotationSpeed);
            this.rigidbody.MovePosition(this.transform.position + direction * pickupSpeed);
        }
	}


    void OnTriggerEnter(Collider col)
    {
        if (transform.parent.gameObject != null)
        {
            if (col.gameObject == transform.parent.gameObject)
            {
                Health healthScript = Ship.GetComponent<Health>();
                healthScript.health = Mathf.Min(healthScript.maxHealth, healthScript.health + healthToAdd);
                Destroy(this.gameObject);
            }
        }
    }
}
