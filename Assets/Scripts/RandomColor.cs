using UnityEngine;
using System.Collections;

public class RandomColor : MonoBehaviour {

	public Color[] altColors;
	
	// Use this for initialization
	void Start () {
		int index = Random.Range(0, altColors.Length);
		
		if (index < altColors.Length) {
			Color color = altColors[index];
			renderer.material.color = color;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
