using UnityEngine;
using System.Collections;

public class TimerBarBehaviour : MonoBehaviour {
	
	public Transform bar;
	public TextMesh text;
	public GameObject[] rocketDisplay;
	
	public Color fullColor;
	public Color emptyColor;
	
	public float shakeSpeed = 4f;
	public float shakeMagnitude = 0.05f;
	
	private Vector3 parentInitialScale;
	private Vector3 initialScale;
	private Vector3 initialPosition;
	private float shakeTime;
	
	// Use this for initialization
	void Start () {
		parentInitialScale = transform.localScale;
		initialScale = bar.localScale;
		initialPosition = bar.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		if (shakeTime > 0f) {
			shakeTime -= Time.deltaTime;
			
			float scaleMult = Mathf.Sin(Time.timeSinceLevelLoad * Mathf.PI * shakeSpeed) * shakeMagnitude + 1f;
			transform.localScale = parentInitialScale * scaleMult;
		}
	}
	
	public void setStatus(float fraction, float value) {
		bar.transform.localScale = new Vector3(initialScale.x * fraction, initialScale.y, initialScale.z);
		bar.transform.localPosition = new Vector3(initialPosition.x + (1f - fraction) * initialScale.x / 2f, initialPosition.y, initialPosition.z);
		bar.renderer.material.color = Color.Lerp(emptyColor, fullColor, fraction);
		bar.renderer.enabled = fraction > 0f;
		text.text = value.ToString("F2");
		text.color = Color.Lerp(emptyColor, fullColor, fraction);
	}
	
	public void setRocketCount(int rockets) {
		Mathf.Clamp(rockets, 0, 5);
		
		for (int i = 0; i < 5; i++) {
			rocketDisplay[i].renderer.enabled = (i < rockets);
		}
	}
	
 	public void setShakeTime(float time) {
		shakeTime = time;
	}
}
