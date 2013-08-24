using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(Collider))]
public class RocketBehaviour : MonoBehaviour {
	
	public float rocketLifetime;
	public float turnAngleThreshold;
	public float turnSpeedDegrees;
	public float acceleration;
	
	public ParticleSystem particleSystem;
	public Transform model;
	
	public ExplosionBehaviour explosionPrefab;
	
	private bool controlRocket = false;
	private bool exploded;
	
	// Use this for initialization
	void Start () {
		exploded = false;
		
		StartCoroutine(LifetimeAction());
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
		Explode();
	}
	
	private void Explode() {
		if (!exploded) {
			exploded = true;
			
			ExplosionBehaviour explosion = (ExplosionBehaviour) Instantiate(explosionPrefab, transform.position, Quaternion.identity);
			explosion.Explode();
			
			// remove pigs from rockets
			PigBehaviour[] pigs = GetComponentsInChildren<PigBehaviour>();
			foreach(PigBehaviour pig in pigs) {
				pig.AbandonRocket();
			}
			
			// disable
			particleSystem.Stop();
			collider.enabled = false;
			rigidbody.isKinematic = true;
			Destroy(model.gameObject);
		}
		
		// destroy later
		StartCoroutine(DestroyAction());
	}
	
	private IEnumerator LifetimeAction() {
		yield return new WaitForSeconds(rocketLifetime);
		Explode ();
	}
	
	private IEnumerator DestroyAction() {
		yield return new WaitForSeconds(4f);
		Destroy(gameObject);
	}
}
