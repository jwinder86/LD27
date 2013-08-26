using UnityEngine;
using System.Collections;

public class PersonTriggerBehaviour : TargetTrigger {

	public ParticleSystem blood;
	
	protected override IEnumerator ExplodeLater() {
		blood.Play();
		
		for (int i = 0; i < gibCount; i++) {
			Vector3 dir = Random.onUnitSphere;
			GibBehaviour gib = (GibBehaviour) Instantiate(gibPrefab, transform.position + new Vector3(0f, 1.5f, 0f) + dir, Quaternion.identity);
			gib.rigidbody.velocity = dir * gibSpeed;
		}
		
		// disable
		collider.enabled = false;
		rigidbody.isKinematic = true;
		
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		
		// hide children
		foreach (Renderer renderer in renderers) {
			if (renderer != blood.renderer) {
				renderer.enabled = false;
			}
		}
		
		yield return new WaitForSeconds(2f);
		
		Destroy(gameObject);
	}
}
