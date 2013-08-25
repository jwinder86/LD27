using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]
[RequireComponent (typeof(AudioSource))]
public class TargetTrigger : MonoBehaviour {
	
	public BoredomClock boredomClock;
	public AudioClip destructionSound;
	// Use this for initialization
	void Start () {
		boredomClock = (BoredomClock) FindObjectOfType(typeof(BoredomClock));
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Explode(){
		Debug.Log ("exploding");
		audio.PlayOneShot(destructionSound);
		boredomClock.increaseClock(10);
		rigidbody.useGravity = true;
		rigidbody.constraints = RigidbodyConstraints.None;
		rigidbody.AddTorque(Random.rotationUniform.eulerAngles, ForceMode.VelocityChange);
		Destroy(gameObject, 2f);
	}
	
}
