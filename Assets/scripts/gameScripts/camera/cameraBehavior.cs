using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class cameraBehavior : MonoBehaviour{
	public float[] bounds = new float[4]; //the distance from the camera the player can move without causing the camera to move
	public GameObject player; //a player reference

	public float speed; //the speed the camera followes the player at

	// Use this for initialization
	void Start () {
		//player = GameObject.FindGameObjectWithTag ("Player"); //find the player
	}
	
	// Update is called once per frame
	void Update () {

		//see if the player is beyond the bounds if so then start moving to the player
		if (player.transform.position.x < transform.position.x + bounds [0]) { // right
			StartCoroutine (move());
		} else if (player.transform.position.x > transform.position.x + bounds [1]) { // left
			StartCoroutine (move());
		} else if (player.transform.position.y < transform.position.y + bounds [2]) { // top
			StartCoroutine (move());
		} else if (player.transform.position.y > transform.position.y + bounds [3]) { // bottom
			StartCoroutine (move());
		}
	}

	IEnumerator move(){
		//this is to move the camera to the player
		while (transform.position.x < player.transform.position.x - .1
			|| transform.position.x > player.transform.position.x + .1
			|| transform.position.y < player.transform.position.y - .1
			|| transform.position.y > player.transform.position.y + .1) {

			//using vector3.moveTowards to easily glide to the player's position
			transform.position = Vector3.MoveTowards (transform.position, 
				new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z), 
				speed * Time.deltaTime);
			yield return null;
		}
	}
}
