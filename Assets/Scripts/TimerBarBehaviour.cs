using UnityEngine;
using System.Collections;

public class TimerBarBehaviour : MonoBehaviour {
	
	public Transform bar;
	public TextMesh text;
	
	public Color fullColor;
	public Color emptyColor;
	
	private Vector3 initialScale;
	
	// Use this for initialization
	void Start () {
		initialScale = bar.localScale;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void setStatus(float fraction, float value) {
		bar.transform.localScale = new Vector3(initialScale.x * fraction, initialScale.y, initialScale.z);
		bar.renderer.material.color = Color.Lerp(emptyColor, fullColor, fraction);
		text.text = value.ToString("F2");
	}
}
