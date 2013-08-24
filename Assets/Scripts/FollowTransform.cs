using UnityEngine;
using System.Collections;

public class FollowTransform : MonoBehaviour {
	
	public Transform toFollow;
	
	private Vector3 diff;
	
	// Use this for initialization
	void Start () {
		diff = transform.position - toFollow.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = toFollow.position + diff;
	}
}
