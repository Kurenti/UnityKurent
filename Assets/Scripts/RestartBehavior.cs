using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartBehavior : MonoBehaviour {

	public Button restartButton;

	// Use this for initialization
	void Start () {
		restartButton.onClick.AddListener(TaskOnClick);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void TaskOnClick() {
		SceneManager.LoadScene(0);
	}
}
