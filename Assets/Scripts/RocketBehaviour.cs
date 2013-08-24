using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(Collider))]
public class RocketBehaviour : MonoBehaviour {
	
	public float turnAngleThreshold;
	public float turnSpeedDegrees;
	public float acceleration;
	
	public ExplosionBehaviour explosionPrefab;
	
	private bool controlRocket = false;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// thrust forward
		Vector3 direction = transform.forward;
		rigidbody.AddForce(direction * acceleration, ForceMode.Acceleration);
		
		// rotate
		//Debug.Log(controlRocket);
		if (controlRocket) {
			if (Input.GetAxis("Horizontal") < 0) {
				transform.RotateAround(transform.position, -transform.right, turnSpeedDegrees * Time.deltaTime);
			} else if (Input.GetAxis("Horizontal") > 0) {
				transform.RotateAround(transform.position, -transform.right, -turnSpeedDegrees * Time.deltaTime);
			}
		}
	}
	
	public void SetControlRocket(bool control) {
		this.controlRocket = control;
		Debug.Log ("Control: " + controlRocket);
	}
	
	public void OnCollisionEnter(Collision collision) {
		ExplosionBehaviour explosion = (ExplosionBehaviour) Instantiate(explosionPrefab, transform.position, Quaternion.identity);
		explosion.Explode();
		Destroy(gameObject);
	}
}
