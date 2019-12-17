using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;

public class fortBehavior : MonoBehaviour {
	public List<GameObject> cannons = new List<GameObject>(); //list of cannons on the fort
	private Transform cParent; //the parent of all the cannons
	private bool disabled = false; //if the fort can still defend this is false
	public GameObject flag;
	public GameObject playerFlag;

	// Use this for initialization
	void Start () {
		cParent = transform.Find ("cannons"); //find the parent of the cannons
		for (int i = 0; i < cParent.childCount; i++) {
			cannons.Add (cParent.GetChild (i).gameObject); //add the cannons to the cannon list
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!disabled){//check to make sure not disabled
			for (int i = 0; i < cannons.Count; i++) {
				if (cannons [i] == null) { //if cannon is dead
					cannons.Remove (cannons [i]); //remove that cannon from the list to prevent repeats
					//increment points
					GameObject.Find("gameManager").SendMessage("addPoint");

					PlayGamesPlatform.Instance.IncrementAchievement (GPGSIds.achievement_fort_genocide, 1, (bool success) => {
						Debug.Log("increament fort assault" + success);
					});
					PlayGamesPlatform.Instance.IncrementAchievement (GPGSIds.achievement_fort_obliterator, 1, (bool success) => {
						Debug.Log("increament fort assault" + success);
					});
					if (this.name.Contains ("largeFort")) {
						PlayGamesPlatform.Instance.ReportProgress (GPGSIds.achievement_fort_assault, 100.0f, (bool success) => {
							Debug.Log("fort assault" + success);
						});
					}
				}
			}

			if (cannons.Count <= 0) {
				switchControl ();
			}
		}
	}

	void switchControl(){ //switch control over to the player
		GameObject ci = null;
		Destroy(flag);
		Instantiate (playerFlag, transform.position, transform.rotation, transform);
		for (int i = 0; i < GameObject.FindGameObjectsWithTag ("Backgrounds").Length; i++) {
			GameObject b = GameObject.FindGameObjectsWithTag ("Backgrounds") [i];
			for (int e = 0; e < b.GetComponent<backgroundBehavior> ().objects.Count; e++) {
				GameObject o = b.GetComponent<backgroundBehavior> ().objects [e];
				if (o.name == this.name) {
					ci = (GameObject)Instantiate (b.GetComponent<backgroundBehavior> ().capturedIcon, 
						transform.position, transform.rotation, transform);
					ci.layer = LayerMask.NameToLayer ("icons");
					b.GetComponent<backgroundBehavior> ().objects.Remove (this.gameObject);
					b.GetComponent<backgroundBehavior> ().capturedIslands.Add (this.gameObject);
					b.GetComponent<backgroundBehavior> ().findNearestIsland ();
				}
			}
		}

		disabled = true;
	}
}
