using UnityEngine;
using System.Collections;

public class BoredomClock : MonoBehaviour {
	
	public float boredomMax = 10f;
	public TimerBarBehaviour display;
	
	private float boredom;
	
	private PigBehaviour pig;
	private bool gameRunning;
	
	public string nextLevel;
	
	// Use this for initialization
	void Start () {
		boredom = boredomMax;
		gameRunning = true;
		
		pig = (PigBehaviour) FindObjectOfType(typeof(PigBehaviour));
	}
	
	// Update is called once per frame
	void Update () {
	 	if (gameRunning) {
			if(boredom > 0f){
				boredom = boredom - Time.deltaTime;
			} else {
				boredom = 0f;
				GameOver();
			}
			
			// don't allow the boredom clock to be higher than boredomMax
			if(boredom > boredomMax){
				boredom = boredomMax;
			}
		}
		if(Input.GetKeyDown ("r")){
			Application.LoadLevel(Application.loadedLevel);
		}
		display.setStatus(boredom / boredomMax, boredom);
	}
	
	
	public void increaseClock(float amount){
		if (gameRunning) {
			boredom += amount;
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
			gameRunning = false;		
			StartCoroutine(loadNextLevel());
		
		
	}
	
	
	private IEnumerator loadNextLevel() {
		yield return new WaitForSeconds(5f);
		Application.LoadLevel(nextLevel);
	}
	
	private IEnumerator ReloadLevel() {
		yield return new WaitForSeconds(5f);
		Application.LoadLevel(Application.loadedLevel);
	}
}
