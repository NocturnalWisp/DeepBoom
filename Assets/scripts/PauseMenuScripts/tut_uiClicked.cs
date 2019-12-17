using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class tut_uiClicked : MonoBehaviour {
	public GameObject gameManager;

	// Use this for initialization
	void Start () {
		EventTrigger trigger = GetComponent<EventTrigger> ();
		EventTrigger.Entry entry = new EventTrigger.Entry ();
		entry.eventID = EventTriggerType.PointerClick;
		entry.callback.AddListener ((data) => {
			clicked ((PointerEventData)data);
		});
		trigger.triggers.Add (entry);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void clicked(PointerEventData data){
		gameManager.SendMessage ("revert");
	}
}
