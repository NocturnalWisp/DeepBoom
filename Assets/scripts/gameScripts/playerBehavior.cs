using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using System;
using UnityEngine.Networking;

[Serializable]
public class upgrade
{
    public string name;
    public bool enabled;

    public upgrade(string n)
    {
        this.name = n;
    }
}

[Serializable]
public class cannonPosition{
	public Transform position;
	public bool left, right, forward, behind;
}

[RequireComponent(typeof(AudioSource))]
public class playerBehavior : MonoBehaviour {
	public bool disabled, paused;
	private float hor, ver; //for inputs

	public GameObject gameMgr;

    [Header("Movement")]
	public float m_speed;
	public float r_speed;//speed

	[Header("Firing")]
	public float fireRate;//time it takes to reload the cannon
	private float nextFire = 0;//ticker to keep track of time past before firing
	public GameObject cannonBallPrefab; //the cannonball

	[Header("Health")]
	public float maxHealth;
	public float health;
	public int damage;
	public Slider healthSlider; //the amount of health the payer has

    [Header("Upgrades")]
    public List<upgrade> upgrades = new List<upgrade>();

	[Header("Cannons")]
	public List<cannonPosition> cannonPositions = new List<cannonPosition>();

	[Header("Misc")]
	public Sprite[] sprites;//the player sprites
	public List<GameObject> ignorecollisions = new List<GameObject>();
	public GameObject fires;
	AudioSource cannonShot;

	// Use this for initialization
	void Start () {
        upgrades.Clear();
        upgrades.Add(new upgrade("broadSideCannons"));

		maxHealth = health;
		healthSlider.value = health;

        Camera.main.GetComponent<cameraBehavior>().player = gameObject;
		fires.SetActive (false);
		cannonShot = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {

		//make sure collision issues don't occur
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
		GetComponent<Rigidbody2D> ().freezeRotation = true;
		if (!disabled && !paused)
        {
            //movement
			if (SystemInfo.deviceType == DeviceType.Handheld) {
				hor = CrossPlatformInputManager.GetAxis ("Horizontal");
				ver = CrossPlatformInputManager.GetAxis ("Vertical");
			} else {
				hor = Input.GetAxis ("Horizontal");
				ver = Input.GetAxis ("Vertical");
			}

            //move the player over time times speed in the direction given
            transform.Translate(hor * m_speed * Time.deltaTime, ver * m_speed * Time.deltaTime, 0, Space.World);

            //face the proper dierection according to the movement
            checkDirection();

            //if fire button pressed
			if (Input.GetKeyUp(KeyCode.Space))
            {
                fire();
            }
        }
    }

	void checkDirection(){
        if (hor != 0 || ver != 0)
        {
            //rotate the player in the direction of input given
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Atan2(hor, -ver) * Mathf.Rad2Deg);
        }
    }

	void fire(){

        //fire the cannon if ready
        if (Time.time > nextFire)
        {
			for (int x = 0; x < cannonPositions.Count; x++) {
				//keep time to fire in check
				nextFire = fireRate + Time.time;
				cannonShot.Play ();

				//create the cannonball
				GameObject cb = null;
				if (cannonPositions [x].right) {
					cb = (GameObject)Instantiate (cannonBallPrefab, cannonPositions [x].position.position, transform.rotation);
					cb.GetComponent<cannonBallBehavior>().startForce(1);
				}else if (cannonPositions [x].left) {
					cb = (GameObject)Instantiate (cannonBallPrefab, cannonPositions [x].position.position, transform.rotation);
					cb.GetComponent<cannonBallBehavior>().startForce(2);
				}else if (cannonPositions [x].forward) {
					cb = (GameObject)Instantiate (cannonBallPrefab, cannonPositions [x].position.position, transform.rotation);
					cb.GetComponent<cannonBallBehavior>().startForce(3);
				}else if (cannonPositions [x].behind) {
					cb = (GameObject)Instantiate (cannonBallPrefab, cannonPositions [x].position.position, transform.rotation);
					cb.GetComponent<cannonBallBehavior>().startForce(4);
				}
				Physics2D.IgnoreCollision (cb.GetComponent<Collider2D> (), this.gameObject.GetComponent<Collider2D> ());
				cb.GetComponent<cannonBallBehavior> ().damage = damage;

				for (int i = 0; i < ignorecollisions.Count; i++) {
					for (int e = 0; e < ignorecollisions [i].GetComponents<PolygonCollider2D> ().Length; e++) {
						Physics2D.IgnoreCollision (cb.GetComponent<Collider2D> (), ignorecollisions [i].GetComponents<PolygonCollider2D> () [e]);
					}
				}

				cb.GetComponent<cannonBallBehavior> ().shotFrom = gameObject;

				//destroy the cannonball after a certain amount of time
				Destroy (cb, 2.0f);
			}
        }
	}

	void takeDamage(int amount){
		health -= amount; //decrese it by amount
		fires.SetActive(false);

		//change sprites at a certain health level
		if (health <= 0) {
			GetComponent<SpriteRenderer> ().sprite = sprites [3];
			died (); //die command
			fires.SetActive(true);
		} else if (health <= maxHealth * 0.4f) {
			GetComponent<SpriteRenderer> ().sprite = sprites [2];
			fires.SetActive(true);
		} else if (health <= maxHealth * 0.8f) {
			GetComponent<SpriteRenderer> ().sprite = sprites [1];
		}else if (health <= maxHealth) {
			GetComponent<SpriteRenderer> ().sprite = sprites [0];
		}
		updateHealth ();
	}

	public void heal(float amount){
		health += amount; //increase it by amount
		fires.SetActive(false);

		//change sprites at a certain health level
		if (health <= maxHealth * 0.4f) {
			GetComponent<SpriteRenderer> ().sprite = sprites [2];
			fires.SetActive(true);
		} else if (health <= maxHealth * 0.8f) {
			GetComponent<SpriteRenderer> ().sprite = sprites [1];
		} else if (health <= maxHealth) {
			GetComponent<SpriteRenderer> ().sprite = sprites [0];
		} else {
			//change to max if past max
			health = maxHealth;
			GetComponent<SpriteRenderer> ().sprite = sprites [0];
		}
		updateHealth();
	}

	public void updateHealth(){
		//change the health text to that number
		healthSlider.value = health;
	}

	void died(){
		//dead code
		disabled = true;
		gameMgr.SendMessage ("gameOver");
	}
}
