using UnityEngine;
using System.Collections;

[RequireComponent (typeof(ParticleSystem))]
[RequireComponent (typeof(AudioSource))]
[RequireComponent (typeof(Collider))]
public class ExplosionBehaviour : MonoBehaviour {
	
	public AudioClip explosionSound;
	
	public float explosionForce;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void Explode() {
		StartCoroutine(ExplodeAction());
	}
	
	private IEnumerator ExplodeAction() {
		particleSystem.Play();
		audio.PlayOneShot(explosionSound);
		
		yield return new WaitForSeconds(0.2f);
		
		collider.enabled = false;
		
		yield return new WaitForSeconds(2f);
		
		Destroy(gameObject);
	}
	
	public void OnTriggerEnter(Collider other) {
		PigBehaviour pig = other.GetComponent<PigBehaviour>();
		TargetTrigger target = other.GetComponent<TargetTrigger>();
		
		Debug.Log(pig + "," + target);
		
		if (pig != null) {
			pig.Stun();
		}
		
		if (target != null){
			Debug.Log ("Target!");
			target.Explode();
		}
		
		if (other.rigidbody != null) {
			Vector3 direction = (other.transform.position - transform.position).normalized;
			
			other.rigidbody.AddForce(direction * explosionForce, ForceMode.VelocityChange);
		}
	}
}
