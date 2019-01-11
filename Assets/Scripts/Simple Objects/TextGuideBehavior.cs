using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextGuideBehavior : MonoBehaviour {

	public Text guide;

	private string[] LINES = {
		"ASDFASDF"
	};

	// Use this for initialization
	void Start () {
		guide.text = LINES[0];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
