using UnityEngine;
using System.Collections;

[RequireComponent (typeof(ParticleSystem))]
public class ExplosionBehaviour : MonoBehaviour {

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
		
		yield return new WaitForSeconds(2);
		
		Destroy(gameObject);
	}
}
