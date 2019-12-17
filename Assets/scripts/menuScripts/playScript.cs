using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using GooglePlayGames;

public class playScript : MonoBehaviour {
	Button b;

	// Use this for initialization
	void Start () {
		b = GetComponent<Button> ();
		b.onClick.AddListener (delegate {
			clicked();
		});
	}

	void Awake(){
		StartCoroutine (ShowAd());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void clicked(){
		PlayGamesPlatform.Instance.ReportProgress (GPGSIds.achievement_welcome, 100.0f, (bool success) => {
			if (success){
				Debug.Log("Yay");
			}
		});
		UnityEngine.SceneManagement.SceneManager.LoadScene ("GameSelect");
	}

	IEnumerator ShowAd(){
		while (!Advertisement.IsReady ()) {
			yield return null;
		}
		Advertisement.Show ();
	}
}
