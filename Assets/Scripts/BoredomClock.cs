using UnityEngine;
using System.Collections;

public class BoredomClock : MonoBehaviour {
	
	float boredom = 10;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	 	boredom = boredom - Time.smoothDeltaTime;
		Debug.Log(boredom);
		
		if(Input.GetButton("Jump")){
			boredom += 5;
			Debug.Log("more time");
		}
		
		if(boredom > 10){
			boredom = 10;
		}
		
		if (boredom < 0){
			Debug.Log("DEAD");
		}
	}
}
