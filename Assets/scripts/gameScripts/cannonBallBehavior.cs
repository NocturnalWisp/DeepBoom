using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonBallBehavior : MonoBehaviour {
	public int damage;

	public float speed; //the speed the cannon ball travels
	public GameObject explosionPrefab; //the explosion animation/sprites after impact 
	public GameObject shotFrom; //a variable containing the data of the object which shot this cannonball

	// Use this for initialization
	void Start () {
		//move the cannonball forward at a constant velocity
		if (shotFrom.tag != "Player") {
			GetComponent<Rigidbody2D> ().AddForce (-transform.up * speed);
		}
	}

	public void startForce(int dir){
		if (dir == 1) {//right
			GetComponent<Rigidbody2D> ().AddForce (transform.right * speed);
		}else if (dir == 2) {//left
			GetComponent<Rigidbody2D> ().AddForce (-transform.right * speed);
		}else if (dir == 3) {//forward
			GetComponent<Rigidbody2D> ().AddForce (-transform.up * speed);
		}else if (dir == 4) {//behind
			GetComponent<Rigidbody2D> ().AddForce (transform.up * speed);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<Rigidbody2D> ().velocity.x == 0 && GetComponent<Rigidbody2D> ().velocity.y == 0) {
			Destroy (this.gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D c){
		//check to make sure a collision with an enemy or a player occured
		if (c.gameObject.tag != "Untagged" && c.gameObject.tag != "Land") {
			if (c.gameObject.GetComponent<enemyBehavior> () != null) {//check if enemy
				c.gameObject.GetComponent<enemyBehavior> ().hitBy = shotFrom; //make sure the enemy knows who hit it
			}
			c.gameObject.SendMessage ("takeDamage", damage); //tell the object to take damage

			//create explosion at contact point
			Instantiate (explosionPrefab, transform.position, transform.rotation);

			Destroy (this.gameObject);//remove the cannonball object
		}
	}
}
