using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;

public class gameManager : MonoBehaviour {
	public GameObject[] ui;
	public GameObject pauseMenu, tutorialMenu, points, time;
	public List<GameObject> backgrounds = new List<GameObject>();
	public bool loggedin;

	private float t;
	private int kills;
	private bool once = true;
	private bool stopTime = false;

	// Use this for initialization
	void Start () {
		switch(GameObject.Find("staying object").GetComponent<stayingObjectBehavior>().type){
		case 1:
			t = 300;
			break;
		case 2:
			t = 600;
			break;
		case 3: 
			t = -10;
			break;
		default:
			t = -10;
			break;
		}

		//tut menu stuff
		tutorialMenu.SetActive (true);
		sw2 ();

		for (int i = 0; i < ui.Length; i++) {
			ui [i].GetComponent<CanvasGroup> ().interactable = false;
		}
	}

	void revert(){
		sw21();
		tutorialMenu.SetActive (false);

		for (int i = 0; i < ui.Length; i++) {
			ui [i].GetComponent<CanvasGroup> ().interactable = true;
		}
	}

	void login(){
		//logged in
		loggedin = true;
	}

	void addPoint(){
		kills += 1000;
		points.GetComponent<Text> ().text = kills.ToString ();
		PlayGamesPlatform.Instance.ReportProgress (GPGSIds.achievement_first_kill, 100.0f, (bool success) => {
			Debug.Log("first kill" + success);
		});
	}

	// Update is called once per frame
	void Update () {
		if (once) {
			//pause the game
			if (Input.GetKeyDown (KeyCode.Escape)) {
				sw ();
			}

			if (t == -10){
				//do nothing. It is infinite.
			}else if (t <= 0) {
				timeOver();
				once = false;
			} else {
				if (!stopTime) {
					t -= Time.deltaTime;
					string minutes = Mathf.Floor (t / 60).ToString ("00");
					string seconds = (t % 60).ToString ("00.00");
					time.GetComponent<Text> ().text = minutes + ":" + seconds;
				}
			}
		}
	}

	//if time ends
	void timeOver(){
		GameObject.Find ("staying object").GetComponent<stayingObjectBehavior> ().points = kills;
		UnityEngine.SceneManagement.SceneManager.LoadScene ("GameOver");
	}

	//if player dies
	void gameOver(){
		GameObject.Find ("staying object").GetComponent<stayingObjectBehavior> ().points = kills;
		UnityEngine.SceneManagement.SceneManager.LoadScene ("GameOver");
	}

	//go to menu
	void gotoMenu(){
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Menu");
	}

	IEnumerator wait(float t){
		for (float i = 0; i < t;){
			i+=Time.deltaTime;
			yield return null;
		}
		yield return 1;
	}

	public void sw(){
		for (int i = 0; i < ui.Length; i++) {
			ui [i].SetActive (!ui[i].activeSelf);
		}
		pauseMenu.SetActive (!pauseMenu.activeSelf);
		stopTime = !stopTime;
		for (int i = 0; i < GameObject.FindGameObjectsWithTag ("Enemy1").Length; i++) {
			GameObject.FindGameObjectsWithTag ("Enemy1") [i].GetComponent<enemyBehavior> ().paused = !GameObject.FindGameObjectsWithTag ("Enemy1") [i].GetComponent<enemyBehavior> ().paused;
		}
		for (int i = 0; i < GameObject.FindGameObjectsWithTag ("Enemy2").Length; i++) {
			GameObject.FindGameObjectsWithTag ("Enemy2") [i].GetComponent<enemyBehavior> ().paused = !GameObject.FindGameObjectsWithTag ("Enemy2") [i].GetComponent<enemyBehavior> ().paused;
		}
		for (int i = 0; i < GameObject.FindGameObjectsWithTag ("Enemy3").Length; i++) {
			GameObject.FindGameObjectsWithTag ("Enemy3") [i].GetComponent<enemyBehavior> ().paused = !GameObject.FindGameObjectsWithTag ("Enemy3") [i].GetComponent<enemyBehavior> ().paused;
		}
		for (int i = 0; i < GameObject.FindGameObjectsWithTag ("Enemy4").Length; i++) {
			GameObject.FindGameObjectsWithTag ("Enemy4") [i].GetComponent<enemyBehavior> ().paused = !GameObject.FindGameObjectsWithTag ("Enemy4") [i].GetComponent<enemyBehavior> ().paused;
		}
		for (int i = 0; i < GameObject.FindGameObjectsWithTag ("Enemy5").Length; i++) {
			GameObject.FindGameObjectsWithTag ("Enemy5") [i].GetComponent<enemyBehavior> ().paused = !GameObject.FindGameObjectsWithTag ("Enemy5") [i].GetComponent<enemyBehavior> ().paused;
		}
		GameObject.FindGameObjectWithTag ("Player").GetComponent<playerBehavior> ().paused = !GameObject.FindGameObjectWithTag ("Player").GetComponent<playerBehavior> ().paused;

		for (int i = 0; i < backgrounds.Count; i++) {
			backgrounds[i].GetComponent<backgroundBehavior> ().paused = !backgrounds[i].GetComponent<backgroundBehavior> ().paused;
		}
	}

	public void sw1(){
		for (int i = 0; i < ui.Length; i++) {
			ui [i].SetActive (!ui[i].activeSelf);
		}
		stopTime = !stopTime;
		for (int i = 0; i < GameObject.FindGameObjectsWithTag ("Enemy1").Length; i++) {
			GameObject.FindGameObjectsWithTag ("Enemy1") [i].GetComponent<enemyBehavior> ().paused = !GameObject.FindGameObjectsWithTag ("Enemy1") [i].GetComponent<enemyBehavior> ().paused;
		}
		for (int i = 0; i < GameObject.FindGameObjectsWithTag ("Enemy2").Length; i++) {
			GameObject.FindGameObjectsWithTag ("Enemy2") [i].GetComponent<enemyBehavior> ().paused = !GameObject.FindGameObjectsWithTag ("Enemy2") [i].GetComponent<enemyBehavior> ().paused;
		}
		for (int i = 0; i < GameObject.FindGameObjectsWithTag ("Enemy3").Length; i++) {
			GameObject.FindGameObjectsWithTag ("Enemy3") [i].GetComponent<enemyBehavior> ().paused = !GameObject.FindGameObjectsWithTag ("Enemy3") [i].GetComponent<enemyBehavior> ().paused;
		}
		for (int i = 0; i < GameObject.FindGameObjectsWithTag ("Enemy4").Length; i++) {
			GameObject.FindGameObjectsWithTag ("Enemy4") [i].GetComponent<enemyBehavior> ().paused = !GameObject.FindGameObjectsWithTag ("Enemy4") [i].GetComponent<enemyBehavior> ().paused;
		}
		for (int i = 0; i < GameObject.FindGameObjectsWithTag ("Enemy5").Length; i++) {
			GameObject.FindGameObjectsWithTag ("Enemy5") [i].GetComponent<enemyBehavior> ().paused = !GameObject.FindGameObjectsWithTag ("Enemy5") [i].GetComponent<enemyBehavior> ().paused;
		}
		GameObject.FindGameObjectWithTag ("Player").GetComponent<playerBehavior> ().paused = !GameObject.FindGameObjectWithTag ("Player").GetComponent<playerBehavior> ().paused;

		for (int i = 0; i < backgrounds.Count; i++) {
			backgrounds[i].GetComponent<backgroundBehavior> ().paused = !backgrounds[i].GetComponent<backgroundBehavior> ().paused;
		}
	}

	public void sw2(){
		stopTime = true;
		for (int i = 0; i < GameObject.FindGameObjectsWithTag ("Enemy1").Length; i++) {
			GameObject.FindGameObjectsWithTag ("Enemy1") [i].GetComponent<enemyBehavior> ().paused = true;
		}
		for (int i = 0; i < GameObject.FindGameObjectsWithTag ("Enemy2").Length; i++) {
			GameObject.FindGameObjectsWithTag ("Enemy2") [i].GetComponent<enemyBehavior> ().paused = true;
		}
		for (int i = 0; i < GameObject.FindGameObjectsWithTag ("Enemy3").Length; i++) {
			GameObject.FindGameObjectsWithTag ("Enemy3") [i].GetComponent<enemyBehavior> ().paused = true;
		}
		for (int i = 0; i < GameObject.FindGameObjectsWithTag ("Enemy4").Length; i++) {
			GameObject.FindGameObjectsWithTag ("Enemy4") [i].GetComponent<enemyBehavior> ().paused = true;
		}
		for (int i = 0; i < GameObject.FindGameObjectsWithTag ("Enemy5").Length; i++) {
			GameObject.FindGameObjectsWithTag ("Enemy5") [i].GetComponent<enemyBehavior> ().paused = true;
		}
		GameObject.FindGameObjectWithTag ("Player").GetComponent<playerBehavior> ().paused = true;

		for (int i = 0; i < backgrounds.Count; i++) {
			backgrounds[i].GetComponent<backgroundBehavior> ().paused = true;
		}
	}

	public void sw21(){
		stopTime = false;
		for (int i = 0; i < GameObject.FindGameObjectsWithTag ("Enemy1").Length; i++) {
			GameObject.FindGameObjectsWithTag ("Enemy1") [i].GetComponent<enemyBehavior> ().paused = false;
		}
		for (int i = 0; i < GameObject.FindGameObjectsWithTag ("Enemy2").Length; i++) {
			GameObject.FindGameObjectsWithTag ("Enemy2") [i].GetComponent<enemyBehavior> ().paused = false;
		}
		for (int i = 0; i < GameObject.FindGameObjectsWithTag ("Enemy3").Length; i++) {
			GameObject.FindGameObjectsWithTag ("Enemy3") [i].GetComponent<enemyBehavior> ().paused = false;
		}
		for (int i = 0; i < GameObject.FindGameObjectsWithTag ("Enemy4").Length; i++) {
			GameObject.FindGameObjectsWithTag ("Enemy4") [i].GetComponent<enemyBehavior> ().paused = false;
		}
		for (int i = 0; i < GameObject.FindGameObjectsWithTag ("Enemy5").Length; i++) {
			GameObject.FindGameObjectsWithTag ("Enemy5") [i].GetComponent<enemyBehavior> ().paused = false;
		}
		GameObject.FindGameObjectWithTag ("Player").GetComponent<playerBehavior> ().paused = false;

		for (int i = 0; i < backgrounds.Count; i++) {
			backgrounds[i].GetComponent<backgroundBehavior> ().paused = false;
		}
	}
}
