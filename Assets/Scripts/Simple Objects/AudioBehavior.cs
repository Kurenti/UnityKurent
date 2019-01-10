using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBehavior : MonoBehaviour {
	
	private AudioSource elevatorMusic;
	private AudioSource steps;
	private AudioSource jump;
	
	// Use this for initialization
	void Start () {
		AudioSource[] sources = GetComponents<AudioSource>();
		elevatorMusic = sources[0];
		steps = sources[1];
		jump = sources[2];
		
		elevatorMusic.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void PlaySteps() {
		if (!steps.isPlaying) {
			steps.Play();
		}
	}
	
	public void StopSteps() {
		if (steps.isPlaying) {
			steps.Stop();
		}
	}
	
	public void PlayJump(float delay) {
		if (!jump.isPlaying) {
			jump.PlayDelayed(delay);
		}
	}
}
