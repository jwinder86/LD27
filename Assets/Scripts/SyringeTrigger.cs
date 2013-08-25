using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class SyringeTrigger : MonoBehaviour {
	
	public AudioClip powerupSound;
	private BoredomClock boredomClock;
	// Use this for initialization
	void Start () {
		boredomClock = (BoredomClock) FindObjectOfType(typeof(BoredomClock));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	 void OnTriggerEnter(Collider other) {
		if(null != other.GetComponent("PigBehaviour")){
			Debug.Log("more time!");
			audio.PlayOneShot(powerupSound);
			boredomClock.WinGame();
			Destroy(gameObject,0.2f);
		}
    }
	
	
}
