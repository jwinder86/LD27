using UnityEngine;
using System.Collections;

public class TargetTrigger : MonoBehaviour {
	
	public BoredomClock boredomClock;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	 void OnCollisionEnter(Collision collision) {
		boredomClock.increaseClock(10);
		Destroy(gameObject);
    }
	
	
}
