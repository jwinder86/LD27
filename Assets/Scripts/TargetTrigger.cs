using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]
[RequireComponent (typeof(AudioSource))]
public class TargetTrigger : MonoBehaviour {
	
	public AudioClip destructionSound;
	
	public ExplosionBehaviour explosionPrefab;
	
	public GibBehaviour gibPrefab;
	public int gibCount = 3;
	public float gibSpeed = 3f;
	
	private BoredomClock boredomClock;
	private bool exploded;
	
	// Use this for initialization
	void Start () {
		boredomClock = (BoredomClock) FindObjectOfType(typeof(BoredomClock));
		exploded = false;
	}
	
	// Update is called once per frame
	void Update () {
		rigidbody.WakeUp();
	}
	
	public void Explode(){
		if (!exploded) {
			exploded = true;
			
			audio.PlayOneShot(destructionSound);
			boredomClock.increaseClock(10);
			
			// activate physics
			rigidbody.useGravity = true;
			rigidbody.constraints = RigidbodyConstraints.None;
			rigidbody.AddTorque(Random.rotationUniform.eulerAngles, ForceMode.VelocityChange);
			
			StartCoroutine(ExplodeLater());
		}
	}
	
	protected virtual IEnumerator ExplodeLater() {
		yield return new WaitForSeconds(2f);
		
		ExplosionBehaviour explosion = (ExplosionBehaviour) Instantiate(explosionPrefab, transform.position, Quaternion.identity);
		explosion.transform.localScale = transform.localScale;
		explosion.Explode();
		
		for (int i = 0; i < gibCount; i++) {
			Vector3 dir = Random.onUnitSphere;
			GibBehaviour gib = (GibBehaviour) Instantiate(gibPrefab, transform.position + new Vector3(0f, 1.5f, 0f) + dir, Quaternion.identity);
			gib.rigidbody.velocity = dir * gibSpeed;
		}
		
		Destroy(gameObject);
	}
	
}
