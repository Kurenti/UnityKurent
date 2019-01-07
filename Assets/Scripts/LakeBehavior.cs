using System.Collections;
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
		Debug.Log("In the lake");
		Debug.Log(SceneManager.sceneCount);
		SceneManager.LoadScene(1);
	}
}
