using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class p_worldBackBehavior : MonoBehaviour {
	public GameObject gamemgr;
	public GameObject mapCamera, mainCamera;
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
		gamemgr.SendMessage ("sw1");
		mainCamera.SetActive (!mainCamera.activeSelf);
		mapCamera.SetActive (!mapCamera.activeSelf);
	}
}
