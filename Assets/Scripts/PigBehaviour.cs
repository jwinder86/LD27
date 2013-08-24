using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(Collider))]
public class PigBehaviour : MonoBehaviour {
	
	public float runSpeed;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		// move left and right
		if (Input.GetAxis("Horizontal") < 0f) {
			rigidbody.velocity = new Vector3(-runSpeed, rigidbody.velocity.y, 0f);
		} else if (Input.GetAxis("Horizontal") > 0f) {
			rigidbody.velocity = new Vector3(runSpeed, rigidbody.velocity.y, 0f);
		}
	}
}
