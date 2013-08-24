using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(Collider))]
public class RocketBehaviour : MonoBehaviour {
	
	public float turnAngleThreshold;
	public float turnSpeedDegrees;
	public float acceleration;
	private Vector3 screenCenter;
	
	// Use this for initialization
	void Start () {
		screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		// thrust forward
		Vector3 direction = transform.forward;
		rigidbody.AddForce(direction * acceleration, ForceMode.Acceleration);
		
		Debug.Log(direction);
		
		// rotate towards mouse
		if (Input.GetAxis("Horizontal") < 0) {
			transform.RotateAround(transform.position, -transform.right, turnSpeedDegrees * Time.deltaTime);
		} else if (Input.GetAxis("Horizontal") > 0) {
			transform.RotateAround(transform.position, -transform.right, -turnSpeedDegrees * Time.deltaTime);
		}
		
		//rigidbody.velocity = rigidbody.velocity.magnitude * transform.forward;
	}
}
