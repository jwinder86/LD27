using UnityEngine;
using System.Collections;

public class BoredomClock : MonoBehaviour {
	
	public float boredomMax = 10f;
	public TimerBarBehaviour display;
	
	private float boredom;
	
	// Use this for initialization
	void Start () {
		boredom = boredomMax;
	}
	
	// Update is called once per frame
	void Update () {
	 	
		if(boredom > 0f){
			boredom = boredom - Time.deltaTime;
		} else {
			boredom = 0f;
		}
		
		// don't allow the boredom clock to be higher than boredomMax
		if(boredom > boredomMax){
			boredom = boredomMax;
		}
		
		display.setStatus(boredom / boredomMax, boredom);
	}
	
	
	public void increaseClock(float amount){
		boredom += amount;
		Debug.Log("more time: " + amount);
	}
	
	

	
}
