using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;

[RequireComponent(typeof(AudioSource))]
public class cannonBehavior : MonoBehaviour {
	public int damage;

	public float area, r_speed, reloadTime;
	public GameObject cannonBallPrefab;
	public bool vertical, positive, up, disabled;

	private float fireTicker = 0; //a ticker that increases every frame
	AudioSource cannonShot;

	// Use this for initialization
	void Start () {
		cannonShot = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!disabled) {

			Collider2D[] circlecast = Physics2D.OverlapCircleAll (transform.position, area);

			for (int i = 0; i < circlecast.Length; i++) {
				if (circlecast [i].tag == "Player") {
					GameObject player = circlecast [i].gameObject;

					if (vertical && up) {
						if (Vector2.Dot (-Vector2.up, player.transform.position - transform.position) >= .3) {
							Vector3 dir = player.transform.position - transform.position;
							float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
							transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0, 0, angle), r_speed * Time.deltaTime);

							fire ();
						}
					} else if (vertical && !up) {
						if (Vector2.Dot (-Vector2.up, player.transform.position - transform.position) <= .3) {
							Vector3 dir = player.transform.position - transform.position;
							float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
							transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0, 0, angle), r_speed * Time.deltaTime);

							fire ();
						}
					} else if (positive) {
						if (Vector2.Dot (-Vector2.right, player.transform.position - transform.position) >= .3) {
							Vector3 dir = player.transform.position - transform.position;
							float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
							transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0, 0, angle), r_speed * Time.deltaTime);

							fire ();
						}
					} else if (!positive) {
						if (Vector2.Dot (-Vector2.right, player.transform.position - transform.position) <= .3) {
							Vector3 dir = player.transform.position - transform.position;
							float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
							transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0, 0, angle), r_speed * Time.deltaTime);

							fire ();
						}
					}
				}
			}
		}
	}

	void fire(){
		//fire the cannon if ready
		if (Time.time > fireTicker) {
			//keep time to fire in check
			fireTicker = reloadTime + Time.time;
			cannonShot.Play ();

			//create a cannonball at this position
			GameObject cb = (GameObject)Instantiate (cannonBallPrefab, transform.position, transform.rotation);
			//make sure the cannonball does not collide wiiith the shooter
			for (int i = 0; i < transform.parent.parent.GetComponents<PolygonCollider2D> ().Length; i++) {
				Physics2D.IgnoreCollision (cb.GetComponent<Collider2D> (), transform.parent.parent.GetComponents<PolygonCollider2D> ()[i]);
			}

			//store a variable from who shot it
			cb.GetComponent<cannonBallFortBehavior> ().shotFrom = this.gameObject;
			cb.GetComponent<cannonBallFortBehavior> ().damage = damage;

			//destroy the cannonball after a certain number of seconds
			Destroy (cb, 2.0f);
		}
	}

	void takeDamage(){
		Destroy (this.gameObject);
	}
}
