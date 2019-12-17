
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class playGamesButton : MonoBehaviour {
	Button b;
	public GameObject gameManager;
	public string id = "CgkIiqG5n-EKEAIQAA";
	public Text signInButtonText, authStatus;

	// Use this for initialization
	void Start () {
		b = GetComponent<Button> ();
		b.onClick.AddListener (delegate() {
			clicked ();
		});
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder ().Build ();

		PlayGamesPlatform.DebugLogEnabled = true;

		PlayGamesPlatform.InitializeInstance (config);
		PlayGamesPlatform.Activate ();

		PlayGamesPlatform.Instance.Authenticate(SignInCallback, false);
	}

	public void ShowAchievements() {
		if (PlayGamesPlatform.Instance.localUser.authenticated) {
			PlayGamesPlatform.Instance.ShowAchievementsUI();
		}
		else {
			Debug.Log("Cannot show Achievements, not logged in");
		}
	}
	

	public void SignInCallback(bool success) {
		if (success) {
			Debug.Log("(Lollygagger) Signed in!");

			// Change sign-in button text
			signInButtonText.text = "Sign out";

			// Show the user's name
			authStatus.text = "Signed in as: " + Social.localUser.userName;
		} else {
			Debug.Log("(Lollygagger) Sign-in failed...");

			// Show failure message
			signInButtonText.text = "Sign in";
			authStatus.text = "Sign-in failed";
		}
	}

	public void SignIn() {
		if (!PlayGamesPlatform.Instance.localUser.authenticated) {
			// Sign in with Play Game Services, showing the consent dialog
			// by setting the second parameter to isSilent=false.
			PlayGamesPlatform.Instance.Authenticate(SignInCallback, false);
		} else {
			// Sign out of play games
			PlayGamesPlatform.Instance.SignOut();

			// Reset UI
			signInButtonText.text = "Sign In";
			authStatus.text = "";
		}
	}

	void Awake(){
		
	}

	void clicked(){
		SignIn ();
	}
	// Update is called once per frame
	void Update () {
		
	}
}
