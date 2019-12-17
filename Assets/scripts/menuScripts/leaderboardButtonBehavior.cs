using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;

public class leaderboardButtonBehavior : MonoBehaviour {
	Button b;

	// Use this for initialization
	void Start () {
		b = GetComponent<Button> ();
		b.onClick.AddListener (delegate() {
			clicked();
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void clicked(){
		if (PlayGamesPlatform.Instance.localUser.authenticated) {
			PlayGamesPlatform.Instance.ShowLeaderboardUI ();
		} else {
			//not working
		}
	}
}
