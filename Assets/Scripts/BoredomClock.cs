using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class BoredomClock : MonoBehaviour {
	
	private static float[] tickTimes = {5f, 4f, 3f, 2.5f, 2f, 1.5f, 1f, 0.75f, 0.5f, 0.25f, -100f};
	
	public float boredomMax = 10f;
	public TimerBarBehaviour display;
	
	private float boredom;
	
	private PigBehaviour pig;
	private bool gameRunning;
	
	public string[] levelList;
	
	public AudioClip moreTime;
	public AudioClip tickSound;
	
	private int tickIndex;
	
	// Use this for initialization
	void Start () {
		boredom = boredomMax;
		gameRunning = true;
		
		pig = (PigBehaviour) FindObjectOfType(typeof(PigBehaviour));
		
		tickIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {
	 	if (gameRunning) {
			if(boredom > 0f){
				boredom = boredom - Time.deltaTime;
				
				if (boredom < 3f) {
					display.setShakeTime(boredom);
				}
				
			} else {
				boredom = 0f;
				GameOver();
			}
			
			// don't allow the boredom clock to be higher than boredomMax
			if(boredom > boredomMax){
				boredom = boredomMax;
			}
		} else {
			if (boredom <= 0f) {
				display.setShakeTime(1f);
			}
		}
		
		playTick();
		
		if(Input.GetKeyDown ("r")){
			Application.LoadLevel(Application.loadedLevel);
		}
		display.setStatus(boredom / boredomMax, boredom);
	}
	
	
	public void increaseClock(float amount){
		if (gameRunning) {
			boredom += amount;
			display.setShakeTime(0.5f);
			
			resetTick();
			
			audio.PlayOneShot(moreTime);
			
			Debug.Log ("increasing time: " + amount + " = " + boredom);
		}
	}
	
	
	public void GameOver() {
		if (gameRunning) {
			boredom = 0f;
			pig.Die();
			
			StartCoroutine(ReloadLevel());
			
			gameRunning = false;
		}
	}
	
	public bool isGameRunning(){
		return gameRunning;
	}
		
	
	public void WinGame() {
		if (gameRunning) {
			gameRunning = false;		
			StartCoroutine(loadNextLevel());
		}		
	}
	
	
	private IEnumerator loadNextLevel() {
		yield return new WaitForSeconds(5f);
		
		string nextLevel = levelList[0];
		for (int i = 0; i < levelList.Length - 1; i++) {
			if (levelList[i].Equals(Application.loadedLevelName)) {
				nextLevel = levelList[i+1];
			}
		}
		
		Application.LoadLevel(nextLevel);
	}
	
	private IEnumerator ReloadLevel() {
		yield return new WaitForSeconds(5f);
		Application.LoadLevel(Application.loadedLevel);
	}
	
	private void playTick() {
		if (boredom < tickTimes[tickIndex]) {
			audio.PlayOneShot(tickSound);
			tickIndex++;
		}
	}
	
	private void resetTick() {
		for (int i = 0; i < tickTimes.Length; i++) {
			if (tickTimes[i] < boredom) {
				tickIndex = i;
				break;
			}
		}
		
		Debug.Log("New Tick Time" + tickIndex);
	}
}
