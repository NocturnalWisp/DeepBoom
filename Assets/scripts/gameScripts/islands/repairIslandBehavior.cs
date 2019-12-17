using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class repairIslandBehavior : MonoBehaviour {
	public GameObject icon;

	// Use this for initialization
	void Start () {
		Instantiate (icon, transform.parent.position, transform.parent.rotation, transform.parent);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<playerBehavior> ().heal (other.gameObject.GetComponent<playerBehavior> ().maxHealth);
		}
	}
}
