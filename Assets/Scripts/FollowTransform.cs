using UnityEngine;
using System.Collections;

public class FollowTransform : MonoBehaviour {
	
	private static readonly float X_COEF = 5f;
	private static readonly float Y_COEF = 7f;
	
	public Transform toFollow;
	
	public float shakeMagnitude;
	public float shakeSpeed;
	
	public float heavyShakeMagnitude;
	public float heavyShakeSpeed;
	
	public Vector3 offset = Vector3.zero;
	
	private float shakeTimer;
	private float heavyShakeTimer;
	
	// Use this for initialization
	void Start () {
		if (offset == Vector3.zero) {
			offset = transform.position - toFollow.position;
		}
		
		transform.LookAt(toFollow, Vector3.up);
	}
	
	void Update () {
		if (shakeTimer > 0f) {
			shakeTimer -= Time.deltaTime;
		}
		
		if (heavyShakeTimer > 0f) {
			heavyShakeTimer -= Time.deltaTime;
		}
	}
	
	// Update is called once per frame
	void LateUpdate () {
		Vector3 newPosition = toFollow.position + offset;
		
		if (heavyShakeTimer > 0f) {
			newPosition += heavyShakeMagnitude * heavyShakeTimer * new Vector3(Mathf.Sin(Time.time * heavyShakeSpeed * X_COEF), Mathf.Cos(Time.time * heavyShakeSpeed * Y_COEF), 0f);
		}else if (shakeTimer > 0f) {
			newPosition += shakeMagnitude * shakeTimer * new Vector3(Mathf.Sin(Time.time * shakeSpeed * X_COEF), Mathf.Cos(Time.time * shakeSpeed * Y_COEF), 0f);
		}
		
		transform.position = newPosition;
	}
	
	public void ShakeTime(float time) {
		Debug.Log("Shaking for: " + time);
		shakeTimer = time;
	}
	
	public void HeavyShakeTime(float time) {
		Debug.Log("Heavy Shaking for: " + time);
		heavyShakeTimer = time;
	}
}