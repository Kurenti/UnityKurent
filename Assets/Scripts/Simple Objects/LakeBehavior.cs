﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LakeBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
	private void OnTriggerEnter(Collider other) {
        other.GetComponent<PlayerBehavior>().dead = true;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
