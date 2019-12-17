using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class p_continueButtonBehavior : MonoBehaviour {
	Button b;
	public GameObject gameMgr;

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
		gameMgr.GetComponent<gameManager> ().sw ();
	}
}
