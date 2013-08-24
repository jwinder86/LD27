using UnityEngine;
using System.Collections;

[RequireComponent (typeof(ParticleSystem))]
[RequireComponent (typeof(AudioSource))]
public class ExplosionBehaviour : MonoBehaviour {
	
	public AudioClip explosionSound;
	
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
		
		yield return new WaitForSeconds(2);
		
		Destroy(gameObject);
	}
}
