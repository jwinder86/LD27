using UnityEngine;
using System.Collections;

public class BoredomClock : TriggerBehaviour {
	
	float boredom;
	float boredomMax = 11;
	float smallIncrease = 5;
	float largeIncrease = 10;
	TextMesh boredomText;
	// Use this for initialization
	void Start () {
		boredom = boredomMax;
		boredomText = GetComponent<TextMesh>();

	}
	
	// Update is called once per frame
	void Update () {
	 	
		if(boredom > 0){
			boredom = boredom - Time.smoothDeltaTime;
		}
		Debug.Log(boredom);
		
		// TODO: need to update this block to be a real trigger thing
		if(Input.GetButton("Jump")){
			increaseClock(smallIncrease);
		}
		
		// don't allow the boredom clock to be higher than boredomMax
		if(boredom > boredomMax){
			boredom = boredomMax;
		}
		
		if (boredom <= 0.01f){
			Debug.Log("DEAD");
			boredomText.text = "DEAD";
		}else{			
			boredomText.text = ((int)boredom).ToString();
		}
	}
	
	
	void increaseClock(float amount){
		boredom += amount;
		Debug.Log("more time: " + amount);
	}
	
	
	public override void Trigger() {
		increaseClock(smallIncrease);
	}
	
}
