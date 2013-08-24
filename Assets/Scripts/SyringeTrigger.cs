using UnityEngine;
using System.Collections;

public class SyringeTrigger : MonoBehaviour {
	
	public BoredomClock boredomClock;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	 void OnTriggerEnter(Collider other) {
		if(null != other.GetComponent("PigBehaviour")){
			boredomClock.increaseClock(10);
			Destroy(gameObject);
		}
    }
	
	
}
