using UnityEngine;
using System.Collections;

public class EffectBehaviour : MonoBehaviour {
	
	private Animation[] anims;
	
	// Use this for initialization
	void Start () {
		anims = GetComponentsInChildren<Animation>();
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void RunEffect() {
		foreach (Animation anim in anims) {
			anim.Play();
		}
	}
}
