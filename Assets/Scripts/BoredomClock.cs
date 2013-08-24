using UnityEngine;
using System.Collections;

public class BoredomClock : MonoBehaviour {
	
	float boredom;
	float boredomMax = 10;
	float smallIncrease = 5;
	float largeIncrease = 10;
	
	// Use this for initialization
	void Start () {
		boredom = boredomMax;
	}
	
	// Update is called once per frame
	void Update () {
	 	
		if(boredom > 0){
			boredom = boredom - Time.smoothDeltaTime;
		}
		Debug.Log(boredom);
		
		// TODO: need to update this block to be a real trigger thing
		if(Input.GetButton("Jump")){
			boredom += smallIncrease;
			Debug.Log("more time");
		}
		
		// don't allow the boredom clock to be higher than boredomMax
		if(boredom > boredomMax){
			boredom = boredomMax;
		}
		
		if (boredom <= 0.01f){
			Debug.Log("DEAD");
		}
	}
}
