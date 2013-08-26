using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class EndingBehaviour : MonoBehaviour {
	
	public AudioClip menuSelect;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") || Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Return)) {
			audio.PlayOneShot(menuSelect);
			StartCoroutine(LoadLevelSoon("Intro"));
		}
	}
	
	private IEnumerator LoadLevelSoon(string levelName) {
		GetComponent<FadeBehaviour>().FadeOut();
		
		yield return new WaitForSeconds(2f);
		
		Application.LoadLevel(levelName);
	}
}
