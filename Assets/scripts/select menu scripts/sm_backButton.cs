using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sm_backButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Button b = GetComponent<Button> ();
		b.onClick.AddListener (delegate {
			clicked();
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void clicked(){
		GameObject.Find ("staying object").SendMessage ("gotoMenu");
	}
}
