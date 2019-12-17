using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;

public class stayingObjectBehavior : MonoBehaviour {
	public int points;
	public int type; 

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);
	}
	
	// Update is called once per frame
	void Update () {
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name == "Menu") {
			Destroy (this.gameObject);
		}
	}

	void gotoMenu(){
		Destroy (this.gameObject);
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Menu");
	}

	void addPointsLeaderboard(){
		switch(type){
		case 1:
			PlayGamesPlatform.Instance.ReportScore (points, GPGSIds.leaderboard_score_in_five_minutes, (bool success) => {
				if (success) {
					Debug.Log ("success");
				} else {
					Debug.Log ("Failure");
				}
			});
			break;
		case 2:
			PlayGamesPlatform.Instance.ReportScore (points, GPGSIds.leaderboard_score_in_ten_minutes, (bool success) => {
				if (success) {
					Debug.Log ("success");
				} else {
					Debug.Log ("Failure");
				}
			});
			break;
		case 3:
			PlayGamesPlatform.Instance.ReportScore (points, GPGSIds.leaderboard_infinite_time, (bool success) => {
				if (success) {
					Debug.Log ("success");
				} else {
					Debug.Log ("Failure");
				}
			});
			break;
		default:
			PlayGamesPlatform.Instance.ReportScore (points, GPGSIds.leaderboard_infinite_time, (bool success) => {
				if (success) {
					Debug.Log ("success");
				} else {
					Debug.Log ("Failure");
				}
			});
			break;
		}
	}

	void five(){
		type = 1;
	}

	void ten(){
		type = 2;
	}

	void infinite(){
		type = 3;
	}
}