using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.UI;

public class leaderboardAdderBehavior : MonoBehaviour {
	Button b;
	public GameObject gameMgr;

	// Use this for initialization
	void Start () {
		gameMgr = GameObject.Find ("staying object");
		b = GetComponent<Button> ();
		b.onClick.AddListener (delegate() {
			clicked();
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
			
	void clicked(){
		gameMgr.SendMessage ("addPointsLeaderboard");
	}
}
