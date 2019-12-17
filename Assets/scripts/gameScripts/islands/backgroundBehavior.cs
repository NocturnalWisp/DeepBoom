using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;

[System.Serializable]
public class Spawnable{
	//for each of the different islands
	public string name;
	public int amount; //number of islands to generate
	public GameObject prefab; //the island
}

public class backgroundBehavior : MonoBehaviour {
	public bool disabled, paused;
	[Header("Islands")]
	public Spawnable[] spawnables;
	public GameObject[] ships; //the shipson the islands
	public GameObject[] flags; //the flags
	public GameObject directionIndicator;

	[Header("Forts")]
	public Spawnable[] forts;

	[Header("Lists")]
	public List<GameObject> objects = new List<GameObject>(); //list of islands that have not been captured
	public List<GameObject> capturedIslands = new List<GameObject> (); //list of captured islands
	public List<GameObject> misc = new List<GameObject>();//the list of random things spread across the map

	[Header("Misc")]
	public GameObject player; //to find the player's position
	public Spawnable[] miscellanneous; //list of miscellanneous things to spread across the world
	public GameObject cI;
	public GameObject capturedIcon; //icons for discovered and undescovered islands

	public bool posX, posY, negX, negY;

	// Use this for initialization
	void Start () {
		directionIndicator = GameObject.Find ("directionIndicator");
		capturedIcon = cI;
		player = GameObject.FindGameObjectWithTag ("Player");
		//randomly generate islands
		for (int i = 0; i < spawnables.Length; i++) {
			Spawnable sbl = spawnables [i];
			for (int e = 1; e <= sbl.amount; e++) {
				//the game object
				GameObject go = (GameObject)Instantiate (sbl.prefab, new Vector2 (Random.Range (transform.position.x - 400, transform.position.x + 400), 
					Random.Range (transform.position.y - 400, transform.position.y + 400)), 
					new Quaternion(0, 0, Random.Range(0, 360), 0));
				go.transform.parent = this.transform;
				go.transform.rotation = Quaternion.AngleAxis (Random.Range (0, 360), Vector3.forward);

				int shipType = Random.Range (0, ships.Length + 1); //random enemy

				for (int f = 0; f < go.GetComponent<landBehavior> ().shipPositions.Length; f++) {
					if (shipType < 5) { //if 5 then it is the player's type
						Instantiate (ships [shipType], go.GetComponent<landBehavior> ().shipPositions[f].position, 
							go.GetComponent<landBehavior> ().shipPositions[f].rotation, go.transform);
					}
				}

				Instantiate (flags [shipType], go.transform.position, Quaternion.AngleAxis(0, Vector3.forward), go.transform);

				//add island to a list for future use
				if (shipType < 5) {
					objects.Add (go);
				} else if (shipType == 5) {
					capturedIslands.Add (go);
					GameObject ci = (GameObject)Instantiate (capturedIcon, go.transform.position, go.transform.rotation, go.transform);

					//make sure the icon is on the icons layer
					ci.layer = LayerMask.NameToLayer ("icons");
				}
			}
		}

		for (int i = 0; i < forts.Length; i++) {
			Spawnable sbl = forts [i];
			for (int e = 1; e <= sbl.amount; e++) {
				//the game object
				GameObject go = (GameObject)Instantiate (sbl.prefab, new Vector2 (Random.Range (transform.position.x - 400, transform.position.x + 400), 
					Random.Range (transform.position.y - 400, transform.position.y + 400)), new Quaternion(0, 0, Random.Range(0, 360), 0));
				go.transform.parent = this.transform;

				int flagType = Random.Range (0, flags.Length - 1); //random flag

				GameObject f = (GameObject)Instantiate (flags [flagType], go.transform.position, Quaternion.AngleAxis(0, Vector3.forward), go.transform);
				go.GetComponent<fortBehavior> ().flag = f;

				objects.Add (go);
				player.GetComponent<playerBehavior> ().ignorecollisions.Add (go);
			}
		}

		for (int i = 0; i < miscellanneous.Length; i++) {
			for (int e = 0; e < miscellanneous[i].amount; e++) {
				GameObject m = (GameObject)Instantiate (miscellanneous [i].prefab, new Vector2(Random.Range (transform.position.x - 400, transform.position.x + 400), 
					Random.Range (transform.position.y - 400, transform.position.y + 400)),
					new Quaternion(0, 0, Random.Range(0, 360), 0));
				m.transform.parent = this.transform;
				misc.Add (m);
			}
		}

		findNearestIsland ();
		died ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!disabled && !paused) {
			findNearestIsland ();
			updateArrow ();
			checkPlayerPos ();
		}
	}

	void checkPlayerPos(){
		float xDis = player.transform.position.x - transform.position.x;
		float yDis = player.transform.position.y - transform.position.y;

		//check to see if player is beyond bounds
		if ((xDis < 400 || yDis < 400) && (xDis > -400 && yDis > -400)) {
			if (xDis >= 350 && posX == false) {
				GameObject b = transform.parent.GetComponent<backgroundCreator> ().addBackground (transform.position.x + 800, transform.position.y);
				if (b != null) {
					posX = true;
					b.GetComponent<backgroundBehavior> ().negX = true;
				}
			} else if (yDis >= 350 && posY == false) {
				GameObject b = transform.parent.GetComponent<backgroundCreator> ().addBackground (transform.position.x, transform.position.y + 800);
				if (b != null) {
					posY = true;
					b.GetComponent<backgroundBehavior> ().negY = true;
				}
			} else if (xDis <= -350 && negX == false) {
				GameObject b = transform.parent.GetComponent<backgroundCreator> ().addBackground (transform.position.x - 800, transform.position.y);
				if (b != null) {
					negX = true;
					b.GetComponent<backgroundBehavior> ().posX = true;
				}
			} else if (yDis <= -350 && negY == false) {
				GameObject b = transform.parent.GetComponent<backgroundCreator> ().addBackground (transform.position.x, transform.position.y - 800);
				if (b != null) {
					negY = true;
					b.GetComponent<backgroundBehavior> ().posY = true;
				}
			}
		}
	}

	void updateArrow(){
		Vector3 diff = transform.parent.GetComponent<backgroundCreator>().nII.transform.position - player.transform.position;
		diff.Normalize();

		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		//directionIndicator.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        directionIndicator.transform.eulerAngles = new Vector3(directionIndicator.transform.eulerAngles.x,
            directionIndicator.transform.eulerAngles.y, rot_z);
    }

	public void died(){
		int o = 0;
		int dis = 0;

		//for the concuering of the islands
		for (int i = 0; i < objects.Count; i++) {
			GameObject s = objects [i]; //the island being conquered
			for (int e = 0; e < s.transform.childCount; e++) {
				GameObject child = s.transform.GetChild (e).gameObject;
				if (child.GetComponent<enemyBehavior> ()) {
					o += 1;
					if (child.GetComponent<enemyBehavior> ().disabled) { //if enemy dead
						dis += 1;
					}
				}
			}

			if (dis >= o && dis != 0) {
				//destroy the previous flag
				Destroy (s.transform.GetChild (s.transform.childCount - 1).gameObject);

				//create flag and conquered icon
				Instantiate (flags [5], s.transform.position, Quaternion.AngleAxis (0, Vector3.forward), s.transform);
				GameObject ci = (GameObject)Instantiate (capturedIcon, s.transform.position, s.transform.rotation, s.transform);

				//make sure the icon is on the icons layer
				ci.layer = LayerMask.NameToLayer ("icons");

				//change lists for future reference
				objects.Remove (s.gameObject);
				capturedIslands.Add (s.gameObject);

				//place the icon on the nearest island
				findNearestIsland ();

				//increment achievement
				PlayGamesPlatform.Instance.IncrementAchievement (GPGSIds.achievement_mass_controller, 1, (bool success) => {
					Debug.Log("increment mass control " + success);
				});
			} else {
				dis = 0;
				o = 0;
			}
		}
	}

	public void findNearestIsland(){
		List<float> distances = new List<float>(); //list of distances from players
		GameObject obj = null; //a variable that will hold the island

		for (int i = 0; i < objects.Count; i++) {
			//add the distance from the player to each island to the distances list
			distances.Add(Vector3.Distance (objects [i].transform.position, player.transform.position));
		}
		if (distances.Count >= 1) {
			//sort the distances by distance
			distances.Sort ();
			
			//find the island where with the least amount of distance from the player
			for (int i = 0; i < objects.Count; i++) {
				if (Vector3.Distance (objects [i].transform.position, player.transform.position) == distances [0]) {
					obj = objects [i]; //store it in the variable
				}
			}

			if (obj != null) {
				//set the icon to that islands position
				transform.parent.GetComponent<backgroundCreator> ().nII.transform.position = obj.transform.position;
			}
		} else {
			//no islands left
			transform.parent.GetComponent<backgroundCreator> ().nII.transform.position = 
				new Vector3(transform.position.x + 550, transform.position.y, transform.position.z);
		}
	}
}
