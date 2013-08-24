using UnityEngine;
using System.Collections;

public class BoredomClock : MonoBehaviour {
	
	public float boredomMax = 10f;
	public TimerBarBehaviour display;
	
	private float boredom;
	
	private PigBehaviour pig;
	private bool gameRunning;
	
	// Use this for initialization
	void Start () {
		boredom = boredomMax;
		gameRunning = true;
		
		pig = (PigBehaviour) FindObjectOfType(typeof(PigBehaviour));
	}
	
	// Update is called once per frame
	void Update () {
	 	
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
		
		display.setStatus(boredom / boredomMax, boredom);
	}
	
	
	public void increaseClock(float amount){
		boredom += amount;
	}
	
	
	private void GameOver() {
		if (gameRunning) {
			gameRunning = false;
			pig.Die();
		}
	}
	
}
