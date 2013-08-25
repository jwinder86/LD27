using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]
public class TargetTrigger : MonoBehaviour {
	
	public BoredomClock boredomClock;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Explode(){
		Debug.Log ("exploding");
		boredomClock.increaseClock(2);
		Destroy(gameObject);
	}
	
}
