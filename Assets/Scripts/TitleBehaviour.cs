using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class TitleBehaviour : MonoBehaviour {
	
	public AudioClip menuMove;
	public AudioClip menuSelect;
	public AudioClip musicSound;
	
	public Color selectedColor;
	public Color normalColor;
	
	public TextMesh playButton;
	public TextMesh tutorialButton;
	public TextMesh storyButton;
	
	public float transitionTime;
	public Transform cam;
	
	public Transform[] markerList;
	
	private TextMesh selected;
	
	private bool moved;
	private TextMesh[] buttons;
	private int buttonIndex;
	
	private bool storyMode;
	private int storyIndex;
	
	// Use this for initialization
	void Start () {
		selected = playButton;
		buttonIndex = 0;
		moved = false;
		
		buttons = new TextMesh[3];
		buttons[0] = playButton;
		buttons[1] = tutorialButton;
		buttons[2] = storyButton;
		
		Camera.main.transform.position = markerList[0].position;
		Camera.main.transform.rotation = markerList[0].rotation;
		
		storyMode = false;
		storyIndex = 0;
		
		audio.PlayOneShot(musicSound, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		
		// story
		if (storyMode) {
			if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") || Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Return)) {
				if (storyIndex == markerList.Length - 1) {
					storyMode = false;
					StartCoroutine(MoveToNextScene(markerList[storyIndex], markerList[0]));
				} else {
					StartCoroutine(MoveToNextScene(markerList[storyIndex], markerList[storyIndex + 1]));
				}
			}
		
		// menu
		} else {
			selected = buttons[buttonIndex];
			
			playButton.color = normalColor;
			tutorialButton.color = normalColor;
			storyButton.color = normalColor;
			
			selected.color = selectedColor;
			
			if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
				buttonIndex--;
				buttonIndex = (buttonIndex < 0) ? buttons.Length - 1 : buttonIndex;
				
				audio.PlayOneShot(menuMove);
				
			} else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
				buttonIndex++;
				buttonIndex = (buttonIndex >= buttons.Length) ? 0 : buttonIndex;
				
				audio.PlayOneShot(menuMove);
			}
			
			if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") || Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Return)) {
				audio.PlayOneShot(menuSelect);
				
				if (selected == playButton) {
					StartCoroutine(LoadLevelSoon("Level4"));
				} else if (selected == tutorialButton) {
					StartCoroutine(LoadLevelSoon("TutorialScene"));
				} else if (selected == storyButton) {
					storyMode = true;
					
					StartCoroutine(MoveToNextScene(markerList[storyIndex], markerList[storyIndex + 1]));
				}
			}
		}
	}
	
	private IEnumerator LoadLevelSoon(string levelName) {
		GetComponent<FadeBehaviour>().FadeOut();
		
		yield return new WaitForSeconds(2f);
		
		Application.LoadLevel(levelName);
	}
	
	private IEnumerator MoveToNextScene(Transform start, Transform end) {
		audio.PlayOneShot(menuMove);
		
		storyIndex++;
		storyIndex = (storyIndex == markerList.Length) ? 0 : storyIndex;
		
		for (float t = 0; t <= transitionTime; t += Time.deltaTime) {
			cam.position = Vector3.Lerp(start.position, end.position, t / transitionTime);
			cam.rotation = Quaternion.Slerp(start.rotation, end.rotation, t / transitionTime);
			
			yield return null;
		}
		
		cam.position = end.position;
		cam.rotation = end.rotation;
	}
}
