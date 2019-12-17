using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fireButtonHandler : MonoBehaviour {
	Button b;

	// Use this for initialization
	void Start () {
		b = GetComponent<Button> ();
		b.onClick.AddListener (delegate {
			clicked();
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void clicked(){
		GameObject.FindGameObjectWithTag ("Player").SendMessage ("fire");
	}
}
