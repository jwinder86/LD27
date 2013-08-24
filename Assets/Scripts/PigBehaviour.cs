using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(Collider))]
public class PigBehaviour : MonoBehaviour {
	
	public float runSpeed;
	public float jumpSpeed;
	public RocketBehaviour rocketPrefab;
	
	private bool ridingRocket;
	private RocketBehaviour rocket;
	private Vector3 screenCenter;
	
	// Use this for initialization
	void Start () {
		screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
		ridingRocket = false;
		rocket = null;
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 aimDir = (Input.mousePosition - screenCenter).normalized;
		Debug.DrawLine(transform.position, transform.position + aimDir * 4);
		
		if (!ridingRocket) {
			// move left and right
			if (Input.GetAxis("Horizontal") < 0f) {
				rigidbody.velocity = new Vector3(-runSpeed, rigidbody.velocity.y, 0f);
			} else if (Input.GetAxis("Horizontal") > 0f) {
				rigidbody.velocity = new Vector3(runSpeed, rigidbody.velocity.y, 0f);
			}
			
			// jump
			if (Input.GetButtonDown("Jump")) {
				rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpSpeed, 0f);
			}
			
			// fire rocket
			if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) {
				float angle = Mathf.Rad2Deg * Mathf.Atan2(-aimDir.y, aimDir.x);
				FireRocket(angle, Input.GetButtonDown("Fire2"));
			}
			
		} else {
			// jump
			if (Input.GetButtonDown("Jump")) {
				AbandonRocket();
				rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y + jumpSpeed, 0f);
			}
		}
	}
	
	private void FireRocket(float angle, bool ride) {
		rocket = (RocketBehaviour) Instantiate(rocketPrefab, transform.position, Quaternion.Euler(new Vector3(angle, 90f, 0f)));
		rocket.rigidbody.velocity = rigidbody.velocity;
		
		// ride rocket
		if (ride) {
			rocket.SetControlRocket(true);
			ridingRocket = true;
			transform.parent = rocket.transform;
			rigidbody.isKinematic = true;
			transform.localPosition = new Vector3(0f, 1f, 0f);
			transform.localRotation = Quaternion.identity;
		}
	}
	
	private void AbandonRocket() {
		ridingRocket = false;
		transform.parent = null;
		transform.localRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
		rigidbody.isKinematic = false;
		rigidbody.velocity = rocket.rigidbody.velocity;
		
		rocket.SetControlRocket(false);
	}
}
