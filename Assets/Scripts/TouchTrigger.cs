using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchTrigger : MonoBehaviour {
	
	public List<TriggerBehaviour> targets;
	
	void OnTriggerEnter(Collider other) {
		Debug.Log("Trigger Enabled!");
		foreach(TriggerBehaviour trigger in targets) {
			trigger.Trigger();
		}
	}
}
