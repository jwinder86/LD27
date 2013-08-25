using UnityEngine;
using System.Collections;

public class DeathPitBehavior : MonoBehaviour {
	
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	void OnTriggerEnter(Collider other) {
		PigBehaviour pig = other.GetComponent<PigBehaviour>();
		if(null != pig){
			pig.Die();
		}
    }
	
}
