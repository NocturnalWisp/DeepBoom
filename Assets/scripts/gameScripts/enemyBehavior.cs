using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;

[RequireComponent(typeof(AudioSource))]
public class enemyBehavior : MonoBehaviour {
	public float maxHealth, health;
	public int damage;

	public float speed, r_speed, impactDistance; // the speeds and the closest distance the enemy can get to the player
	public float reloadTime; //the amount of time it takes to shoot again
	public bool bigShip; //is the ship bigger or smaller?
	public Sprite[] sprites; //the sprites the ship converts to after being hit
	public bool disabled, paused; //is the ship dead?
	public GameObject gameMgr;

	private GameObject target; //The target the enemy is looking for be it enemy or player
	
	private float fireTicker = 0; //a ticker that increases every frame

	public GameObject hitBy; //who this ship was hit by

	public GameObject cannonBallPrefab, fires; //the cannonball gameobject
	private GameObject firesObject;
	AudioSource cannonShot;

	// Use this for initialization
	void Start () {
		gameMgr = GameObject.Find ("gameManager");
		//set health equal to maxhealth
		maxHealth = health;
		firesObject = (GameObject)Instantiate (fires, transform.position, transform.rotation);
		firesObject.transform.parent = transform;
		firesObject.GetComponent<minimapBehavior> ().player = this.gameObject;
		firesObject.SetActive (false);

		cannonShot = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		//make sure collision effects don't send the ship twirling and flying out into the ocean
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);

		if (!disabled && !paused) { //if not dead
			//circle cast to find any ships in the vicinity
			Collider2D[] hit = Physics2D.OverlapCircleAll (transform.position, 10);

			//check for ships in the area
			for (int i = 0; i < hit.Length; i++) {
				if (target != null && hit [i].name == target.name) { //if target is found
					if (target.GetComponent<enemyBehavior> () != null) { //if enemy
						if (target.GetComponent<enemyBehavior> ().disabled) { //if enemy is dead
							target = null; //make sure they are not a target
						}
					}

					//rotate towards the target
					Vector3 dir = hit [i].transform.position - transform.position;
					float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
					angle += 90;
					transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0, 0, angle), r_speed * Time.deltaTime);

					//if to far from target then move towards the target
					if (target != null && Vector3.Distance (transform.position, target.transform.position) > impactDistance) {
						transform.position += -transform.up * speed * Time.deltaTime;
					}

					//make sure collision does not affect this object
					GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);

					//fire the cannonball
					if (Time.time > fireTicker) {
						fire ();
					}
				} else { //if target is not defined or found in the area
					//check if it is a proper target
					if ((hit [i].tag == "Player" ||
					    hit [i].tag == "Enemy1" ||
					    hit [i].tag == "Enemy2" ||
					    hit [i].tag == "Enemy3" ||
					    hit [i].tag == "Enemy4" ||
					    hit [i].tag == "Enemy5") &&
					    hit [i].tag != tag && hit [i].tag != "Untagged" && hit [i].tag != "Land") {

						//ranomly chose a target
						int r = Random.Range (0, hit.Length); //chose a random person to shoot
						target = hit [r].gameObject;

						if (target.GetComponent<enemyBehavior> () != null) {//check if enemy of player
							if (target.GetComponent<enemyBehavior> ().disabled) {//check if dead
								target = null;//can't target a dead person
							}
						}
					}
				}
				//raycast to sense if there is land in the way
				RaycastHit2D hits = Physics2D.Raycast (transform.position, -transform.up, 2);

				Debug.DrawRay (transform.position, -transform.up * 2);

				if (hits.collider != null && hits.collider.tag == "Land") {
					//move the gameobject away from the land
					transform.position += -transform.right * speed/2 * Time.deltaTime;
				}
			}
				
			if (hit.Length <= 0) {
				//random movement or when player is not in view *incomplete*
			}
		}
	}

	void fire(){
		//keep time to fire in check
		fireTicker = reloadTime + Time.time;
		cannonShot.Play ();

		//create a cannonball at this position
		GameObject cb = (GameObject)Instantiate (cannonBallPrefab, transform.position, transform.rotation);
		//make sure the cannonball does not collide with the shooter
		Physics2D.IgnoreCollision (cb.GetComponent<Collider2D>(), this.gameObject.GetComponent<Collider2D>());

		//store a variable from who shot it
		cb.GetComponent<cannonBallBehavior> ().shotFrom = this.gameObject;
		cb.GetComponent<cannonBallBehavior> ().damage = damage;

		//destroy the cannonball after a certain number of seconds
		Destroy (cb, 2.0f);
	}

	void takeDamage(int amount){
		if (!disabled) { //only if alive
			target = hitBy; //who this object was hit by

			health -= amount; //do damage to health
			firesObject.SetActive(false);

			//change sprites at a certain health level
			if (health <= 0) {
				GetComponent<SpriteRenderer> ().sprite = sprites [3];
				if (hitBy.tag == "Player") {
					//increment points
					GameObject.Find("gameManager").SendMessage("addPoint");
					PlayGamesPlatform.Instance.ReportProgress (GPGSIds.achievement_first_kill, 100.0f, (bool success) => {
						Debug.Log("First kill " + success);
					});
					PlayGamesPlatform.Instance.IncrementAchievement (GPGSIds.achievement_mediocre_destroyer, 1, (bool success) => {
						Debug.Log("Increment mediocre destroyer " + success);
					});
					PlayGamesPlatform.Instance.IncrementAchievement (GPGSIds.achievement_enemy_bane, 1, (bool success) => {
						Debug.Log("Increment enemy bane " + success);
					});
				}
				death (); //die command
				firesObject.SetActive(true);
			} else if (health <= maxHealth * 0.3f) {
				GetComponent<SpriteRenderer> ().sprite = sprites [2];
				firesObject.SetActive (true);
			} else if (health <= maxHealth * 0.8f) {
				GetComponent<SpriteRenderer> ().sprite = sprites [1];
			}else if (health <= maxHealth) {
				GetComponent<SpriteRenderer> ().sprite = sprites [0];
			}
		}
	}

	void death(){
		disabled = true; //disable most functions
		for (int i = 0; i < GameObject.FindGameObjectsWithTag ("Backgrounds").Length; i++) {
			GameObject b = GameObject.FindGameObjectsWithTag ("Backgrounds") [i];
			for (int e = 0; e < b.GetComponent<backgroundBehavior> ().objects.Count; e++) {
				GameObject o = b.GetComponent<backgroundBehavior> ().objects [e];
				if (o.transform.Find (this.name) != null) {
					b.GetComponent<backgroundBehavior> ().died ();

				}
			}
		}
	}
}
