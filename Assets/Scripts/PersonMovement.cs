using UnityEngine;
using System.Collections;

public class PersonMovement : MonoBehaviour {
	
	public float personSpeed;
	public float rotateTime = 0.4f;
	public float walkTime = 3f;
	
	private bool walkRight;
	
	// Use this for initialization
	void Start () {
		walkRight = Random.value < 0.5f;
		
		transform.rotation = Quaternion.Euler(new Vector3(0f, walkRight ? 90f : -90f , 0f));
		
		StartCoroutine(ChangeDirection());
	}
	
	// Update is called once per frame
	void Update () {
		if (walkRight) {
			transform.position += new Vector3(personSpeed * Time.deltaTime, 0f, 0f);
		} else {
			transform.position -= new Vector3(personSpeed * Time.deltaTime, 0f, 0f);
		}
	}
	
	private IEnumerator ChangeDirection() {
		yield return new WaitForSeconds(Random.value * walkTime);
		
		while (true) {
			walkRight = !walkRight;
			
			for (float t = 0; t <= rotateTime; t += Time.deltaTime) {
				if (walkRight) {			
					transform.rotation = Quaternion.Slerp(Quaternion.Euler(new Vector3(0f, 270f, 0f)), Quaternion.Euler(new Vector3(0f, 90f, 0f)), t / rotateTime);
				} else {
					transform.rotation = Quaternion.Slerp(Quaternion.Euler(new Vector3(0f, 90f, 0f)), Quaternion.Euler(new Vector3(0f, 270f, 0f)), t / rotateTime);
				}
				
				yield return null;
			}
			
			if (walkRight) {			
				transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
			} else {
				transform.rotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
			}
			
			yield return new WaitForSeconds((Random.value / 2f + 0.5f) * walkTime);
			
		}
	}
}
