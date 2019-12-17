using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class exitButtonBehavior : MonoBehaviour {
	Button b;

	// Use this for initialization
	void Start () {
		b = GetComponent<Button> ();
		b.onClick.AddListener (clicked);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void clicked(){
		Debug.Log ("Exiting");
		Application.Quit ();
	}
}
