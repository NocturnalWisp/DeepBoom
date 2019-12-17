using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionBehavior : MonoBehaviour {
	private Animator anim;//reference to animator

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> (); //get the animator
		StartCoroutine (PlayAnimInterval (.5f, 1)); //play the explosion animation
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private IEnumerator PlayAnimInterval(float n, float time)
	{
		while (n > 0) //player a certain number of times
		{
			anim.Play("explosion", -1, 0F);
			--n; 
			yield return new WaitForSeconds(time);
		}

		//stop the amiation and destroy this animation image
		if (n <= 0) {
			//anim.Stop ();
			Destroy (this.gameObject);
		}
	}
}
