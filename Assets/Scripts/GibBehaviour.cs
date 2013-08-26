using UnityEngine;
using System.Collections;

public class GibBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(GibletAction());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private IEnumerator GibletAction() {
		yield return new WaitForSeconds(2f);
		
		particleSystem.Stop();
		Destroy(rigidbody);
		Destroy(collider);
		
		yield return new WaitForSeconds(2f);
		
		Destroy(gameObject);
	}
}
