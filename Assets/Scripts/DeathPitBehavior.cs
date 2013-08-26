using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]
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
			//if(!pig.rigidbody.isKinematic){
			
			pig.AbandonRocket();
			pig.rigidbody.velocity = pig.rigidbody.velocity / 5f;
				pig.rigidbody.drag = 0.999f;
		//	}
			pig.Die();
		}
    }
	
}
