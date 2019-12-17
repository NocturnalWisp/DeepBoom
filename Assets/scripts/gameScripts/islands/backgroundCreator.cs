using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundCreator : MonoBehaviour {
	public GameObject backgroundPrefab, nearestIslandIcon;
	private GameObject player;
	public GameObject nII;
	public GameObject gameMgr;
	public GameObject worldMapCamera;

	public List<GameObject> backgrounds = new List<GameObject>();

	// Use this for initialization
	void Start () {
		nII = (GameObject)Instantiate (nearestIslandIcon);

		GameObject b = (GameObject)Instantiate (backgroundPrefab, transform);
		b.tag = "Backgrounds";
		backgrounds.Add (b);
		gameMgr.GetComponent<gameManager> ().backgrounds.Add (b);

		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < transform.childCount; i++) {
			if (player.transform.position.x - transform.GetChild(i).position.x >= 450 || player.transform.position.y - transform.GetChild(i).position.y >= 450 ||
				player.transform.position.x - transform.GetChild(i).position.x <= -450 || player.transform.position.y - transform.GetChild(i).position.y <= -450) {
				transform.GetChild (i).gameObject.SetActive (false);
				worldMapCamera.transform.position = new Vector3(transform.GetChild (i).position.x, transform.GetChild (i).position.y, worldMapCamera.transform.position.z) ;
			} else {
				transform.GetChild (i).gameObject.SetActive (true);
				worldMapCamera.transform.position = new Vector3(transform.GetChild (i).position.x, transform.GetChild (i).position.y, worldMapCamera.transform.position.z) ;
			}
		}
	}

	public GameObject addBackground(float x, float y){
		for (int i = 0; i < transform.childCount; i++) {
			if (Vector2.Distance (transform.GetChild (i).position, new Vector2 (x, y)) <= 1) {
				return null;
			}
		}
		GameObject b = (GameObject)Instantiate (backgroundPrefab, new Vector2 (x, y), transform.rotation, transform);
		b.tag = "Backgrounds";
		backgrounds.Add (b);

		gameMgr.GetComponent<gameManager> ().backgrounds.Add (b);
		return b;
	}
}
