using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	private void OnTriggerEnter(Collider other) {
        
		Debug.Log("Triggerred.");
		GameObject bell = GameObject.Find("BellObject");
		GameObject kurent = GameObject.Find("PlayerObject");
		
		bell.transform.parent = kurent.transform;
		bell.transform.localPosition = new Vector3(2.85f, -2.71f, 2.2f);
		
    }

}