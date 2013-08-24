using UnityEngine;
using System.Collections;

public class FeetCollider : MonoBehaviour {
	
	public float groundDuration = 0.2f;
	
	private float onGround;
	
	// Use this for initialization
	void Start () {
		onGround = 0f;
	}
	
	void Update() {
		onGround -= Time.deltaTime;
	}
	
	void OnTriggerEnter(Collider other) {
		onGround = groundDuration;
	}
	
	void OnTriggerStay(Collider other) {
		onGround = groundDuration;
	}
	
	void OnTriggerExit(Collider other) {
		onGround = 0f;
	}
	
	public bool OnGround() {
		return onGround > 0f;
	}
}
