using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour {
	
	public AudioClip[] songs;
	
	// Use this for initialization
	void Start () {
		PlayRandomSong();
	}
	
	// Update is called once per frame
	void Update () {
		if (!audio.isPlaying) {
			PlayRandomSong();
		}
	}
	
	private void PlayRandomSong() {
		AudioClip song = songs[Random.Range(0, songs.Length - 1)];
		
		audio.clip = song;
		audio.Play();
	}
}
