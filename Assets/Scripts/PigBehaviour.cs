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
	
	// Use this for initialization
	void Start () {
		ridingRocket = false;
		rocket = null;
	}
	
	// Update is called once per frame
	void Update () {
		
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
			if (Input.GetButtonDown("Fire1")) {
				FireRocket();
			}
			
		} else {
			// jump
			if (Input.GetButtonDown("Jump")) {
				AbandonRocket();
				rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpSpeed, 0f);
			}
		}
	}
	
	private void FireRocket() {
		rocket = (RocketBehaviour) Instantiate(rocketPrefab, transform.position, Quaternion.Euler(new Vector3(0f, 90f, 0f)));
		rocket.SetControlRocket(true);
		
		// ride rocket
		ridingRocket = true;
		transform.parent = rocket.transform;
		rigidbody.isKinematic = true;
		transform.localPosition = new Vector3(0f, 1f, 0f);
	}
	
	private void AbandonRocket() {
		ridingRocket = false;
		transform.parent = null;
		transform.localRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
		rigidbody.isKinematic = false;
		
		rocket.SetControlRocket(false);
	}
}
