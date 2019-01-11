using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBehavior : MonoBehaviour {
	
	private AudioSource elevatorMusic;
	private AudioSource steps;
	private AudioSource jump;
	private AudioSource attack1;
	private AudioSource attack2;
	private AudioSource attack3;
	private AudioSource attack4;
	private AudioSource bell1;
	private AudioSource bell2;
	
	private bool attackPlaying;
	
	// Use this for initialization
	void Start () {
		AudioSource[] sources = GetComponents<AudioSource>();
		elevatorMusic = sources[0];
		steps = sources[1];
		jump = sources[2];
		attack1 = sources[3];
		attack2 = sources[4];
		attack3 = sources[5];
		attack4 = sources[6];
		bell1 = sources[7];
		bell2 = sources[8];
		
		elevatorMusic.Play();
		
		attackPlaying = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (attack1.isPlaying || attack2.isPlaying || attack3.isPlaying || attack4.isPlaying) {
			attackPlaying = true;
		} else {
			attackPlaying = false;
		}
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
		if (!jump.isPlaying && !attackPlaying) {
			jump.PlayDelayed(delay);
		}
	}
	
	public void PlayAttack1() {
		if (!attackPlaying) {
			attack1.Play();
		}
	}
	
	public void PlayAttack2() {
		if (!attackPlaying) {
			attack2.Play();
		}
	}
	
	public void PlayAttack3() {
		if (!attackPlaying) {
			attack3.Play();
		}
	}
	
	public void PlayAttack4() {
		if (!attackPlaying) {
			attack4.Play();
		}
	}
	
	public void PlayBell1() {
		if (!bell1.isPlaying){
			bell1.Play();
		}
	}

	public void PlayBell2() {
		if (!bell2.isPlaying) {
			bell2.Play();
		}
	}
}
