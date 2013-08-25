using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class SyringeTrigger : MonoBehaviour {
	
	public AudioClip powerupSound;
	public BoredomClock boredomClock;
	// Use this for initialization
	void Start () {
		
		BoredomClock boredomClock = (BoredomClock) FindObjectOfType(typeof(BoredomClock));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	 void OnTriggerEnter(Collider other) {
		if(null != other.GetComponent("PigBehaviour")){
			Debug.Log("more time!");
			audio.PlayOneShot(powerupSound);
			boredomClock.increaseClock(10);
			Destroy(gameObject);
		}
    }
	
	
}
